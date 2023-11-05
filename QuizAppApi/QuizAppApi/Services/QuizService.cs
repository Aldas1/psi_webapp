using QuizAppApi.Dtos;
using QuizAppApi.Dtos.Responses;
using QuizAppApi.Dtos.Requests;
using QuizAppApi.Enums;
using QuizAppApi.Exceptions;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

namespace QuizAppApi.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IExplanationService _explanationService;
    private readonly IQuestionDtoConverterService<SingleChoiceQuestion> _singleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<MultipleChoiceQuestion> _multipleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<OpenTextQuestion> _openTextDtoConverter;
    private readonly IAnswerCheckerService _answerCheckerService;

    public QuizService(
        IQuizRepository quizRepository,
        IExplanationService explanationService,
        IQuestionDtoConverterService<SingleChoiceQuestion> singleChoiceDtoConverter,
        IQuestionDtoConverterService<MultipleChoiceQuestion> multipleChoiceDtoConverter,
        IQuestionDtoConverterService<OpenTextQuestion> openTextDtoConverter,
        IAnswerCheckerService answerCheckerService)
    {
        _quizRepository = quizRepository;
        _explanationService = explanationService;
        _singleChoiceDtoConverter = singleChoiceDtoConverter;
        _multipleChoiceDtoConverter = multipleChoiceDtoConverter;
        _openTextDtoConverter = openTextDtoConverter;
        _answerCheckerService = answerCheckerService;
    }

    public QuizManipulationResponseDto CreateQuiz(QuizManipulationRequestDto request)
    {
        var newQuiz = new Quiz();

        try
        {
            PopulateQuizFromDto(newQuiz, request);
        }
        catch (DtoConversionException e)
        {
            return new QuizManipulationResponseDto { Status = e.Message };
        }

        Quiz? createdQuiz = _quizRepository.AddQuiz(newQuiz);

        if (createdQuiz == null)
        {
            return new QuizManipulationResponseDto { Status = "failed" };
        }

        int createdQuizId = createdQuiz.Id;

        return new QuizManipulationResponseDto { Status = "success", Id = createdQuizId };
    }

    public QuizManipulationResponseDto UpdateQuiz(int id, QuizManipulationRequestDto editRequest)
    {
        var newQuiz = _quizRepository.GetQuizById(id);
        if (newQuiz == null)
        {
            return new QuizManipulationResponseDto { Status = "Quiz not found" };
        }

        try
        {
            PopulateQuizFromDto(newQuiz, editRequest);
        }
        catch (DtoConversionException e)
        {
            return new QuizManipulationResponseDto { Status = e.Message };
        }

        _quizRepository.Save();
        return new QuizManipulationResponseDto { Status = "success", Id = id };
    }


    public IEnumerable<QuestionResponseDto>? GetQuestions(int id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return null;
        }

        var generatedQuestions = new List<QuestionResponseDto>();
        foreach (var question in quiz.Questions)
        {
            var generatedParameters = question switch
            {
                SingleChoiceQuestion singleChoiceQuestion => _singleChoiceDtoConverter.GenerateParameters(
                    singleChoiceQuestion),
                MultipleChoiceQuestion multipleChoiceQuestion => _multipleChoiceDtoConverter.GenerateParameters(
                    multipleChoiceQuestion),
                OpenTextQuestion openTextQuestion => _openTextDtoConverter.GenerateParameters(openTextQuestion),
                _ => null
            };

            if (generatedParameters == null)
            {
                return null;
            }
            generatedQuestions.Add(new QuestionResponseDto
            {
                QuestionText = question.Text,
                Id = question.Id,
                QuestionType = QuestionTypeConverter.ToString(question.Type),
                QuestionParameters = generatedParameters
            });
        }

        return generatedQuestions;
    }

    public IEnumerable<QuizResponseDto> GetQuizzes()
    {
        var quizzes = _quizRepository.GetQuizzes();

        return quizzes.Select(quiz => new QuizResponseDto { Name = quiz.Name, Id = quiz.Id });
    }

    public QuizResponseDto? GetQuiz(int id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return null;
        }

        return new QuizResponseDto { Name = quiz.Name, Id = quiz.Id };
    }

    public async Task<AnswerSubmitResponseDto> SubmitAnswers(int id, List<AnswerSubmitRequestDto> request)
    {
        var response = new AnswerSubmitResponseDto();
        var quiz = _quizRepository.GetQuizById(id);
        var correctAnswers = 0;
        var explanation = "";

        if (quiz == null)
        {
            response.Status = "failed";
            return response;
        }
        else
        {
            response.Status = "success";
        }
        
        var explanations = new List<ExplanationDto>();
        bool correctExplanationAnswer = false;

        foreach (var answer in request)
        {
            var question = quiz.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);

            if (question == null)
            {
                continue;
            }
                
            switch (question)
            {
                case SingleChoiceQuestion singleChoiceQuestion:
                    if (answer.OptionName != null && _answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, answer.OptionName))
                    {
                        correctAnswers++;
                        correctExplanationAnswer = true;
                    }
                    explanation = await _explanationService.GenerateExplanationAsync(singleChoiceQuestion, answer.OptionName ?? answer.AnswerText);
                    break;

                case MultipleChoiceQuestion multipleChoiceQuestion:
                    if (answer.OptionNames != null)
                    {
                        if (_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, answer.OptionNames.Select(optName => new Option { Name = optName }).ToList()))
                        {
                            correctAnswers++;
                            correctExplanationAnswer = true;
                        }
                    }
                    explanation = await _explanationService.GenerateExplanationAsync(multipleChoiceQuestion, answer.OptionNames ?? new List<string>());
                    break;

                case OpenTextQuestion openTextQuestion:
                    var answerText = answer.AnswerText;
                    if (answerText != null && _answerCheckerService.CheckOpenTextAnswer(openTextQuestion, answerText, trimWhitespace: true))
                    {
                        correctAnswers++;
                        correctExplanationAnswer = true;
                    }
                    explanation = await _explanationService.GenerateExplanationAsync(openTextQuestion, answer.AnswerText ?? "");
                    break;
            }
            explanations.Add(new ExplanationDto { QuestionId = question.Id, Explanation = explanation, Correct = correctExplanationAnswer });
        }

        response.CorrectlyAnswered = correctAnswers;
        response.Score = quiz.Questions.Count == 0 ? 0 : (correctAnswers * 100 / quiz.Questions.Count);

        response.Explanations = explanations;
        
        return response;
    }

    public bool DeleteQuiz(int id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return false;
        }

        _quizRepository.DeleteQuiz(id);
        return true;
    }

    private void PopulateQuizFromDto(Quiz quiz, QuizManipulationRequestDto request)
    {
        quiz.Name = request.Name;
        quiz.Questions.Clear();

        foreach (var question in request.Questions)
        {
            Question? generatedQuestion = null;
            switch (QuestionTypeConverter.FromString(question.QuestionType))
            {
                case QuestionType.SingleChoiceQuestion:
                    generatedQuestion = _singleChoiceDtoConverter.CreateFromParameters(question.QuestionParameters);
                    break;
                case QuestionType.MultipleChoiceQuestion:
                    generatedQuestion = _multipleChoiceDtoConverter.CreateFromParameters(question.QuestionParameters);
                    break;
                case QuestionType.OpenTextQuestion:
                    generatedQuestion = _openTextDtoConverter.CreateFromParameters(question.QuestionParameters);
                    break;
            }

            if (generatedQuestion == null)
            {
                throw new DtoConversionException("Failed to create a question");
            }

            generatedQuestion.Text = question.QuestionText;
            quiz.Questions.Add(generatedQuestion);
        }
    }
}