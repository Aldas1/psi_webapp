using Microsoft.AspNetCore.SignalR;
using QuizAppApi.DTOs;
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
    private readonly IQuestionDTOConverterService<SingleChoiceQuestion> _singleChoiceDTOConverter;
    private readonly IQuestionDTOConverterService<MultipleChoiceQuestion> _multipleChoiceDTOConverter;
    private readonly IQuestionDTOConverterService<OpenTextQuestion> _openTextDTOConverter;
    
    public QuizService(
        IQuizRepository quizRepository,
        IExplanationService explanationService,
        IQuestionDTOConverterService<SingleChoiceQuestion> singleChoiceDTOConverter,
        IQuestionDTOConverterService<MultipleChoiceQuestion> multipleChoiceDTOConverter,
        IQuestionDTOConverterService<OpenTextQuestion> openTextDTOConverter)
    {
        _quizRepository = quizRepository;
        _explanationService = explanationService;
        _singleChoiceDTOConverter = singleChoiceDTOConverter;
        _multipleChoiceDTOConverter = multipleChoiceDTOConverter;
        _openTextDTOConverter = openTextDTOConverter;
    }
    public QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request)
    {
        var newQuiz = new Quiz { Name = request.Name };
        foreach (var question in request.Questions)
        {
            Question? generatedQuestion = null;
            switch (QuestionTypeConverter.FromString(question.QuestionType))
            {
                case QuestionType.SingleChoiceQuestion:
                    generatedQuestion = _singleChoiceDTOConverter.CreateFromParameters(question.QuestionParameters);
                    break;
                case QuestionType.MultipleChoiceQuestion:
                    generatedQuestion = _multipleChoiceDTOConverter.CreateFromParameters(question.QuestionParameters);
                    break;
                case QuestionType.OpenTextQuestion:
                    generatedQuestion = _openTextDTOConverter.CreateFromParameters(question.QuestionParameters);
                    break;
            }
            if (generatedQuestion == null)
            {
                return new QuizCreationResponseDTO { Status = "Invalid question data" };
            }
            generatedQuestion.Text = question.QuestionText;
            newQuiz.Questions.Add(generatedQuestion);
        }
        Quiz? createdQuiz = _quizRepository.AddQuiz(newQuiz);
        if (createdQuiz == null)
        {
            return new QuizCreationResponseDTO { Status = "failed" };
        }
        int createdQuizId = createdQuiz.Id;
        return new QuizCreationResponseDTO { Status = "success", Id = createdQuizId };
    }

    public IEnumerable<QuestionResponseDTO>? GetQuestions(int id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return null;
        }

        var generatedQuestions = new List<QuestionResponseDTO>();
        foreach (var question in quiz.Questions)
        {
            var generatedParameters = question switch
            {
                SingleChoiceQuestion singleChoiceQuestion => _singleChoiceDTOConverter.GenerateParameters(
                    singleChoiceQuestion),
                MultipleChoiceQuestion multipleChoiceQuestion => _multipleChoiceDTOConverter.GenerateParameters(
                    multipleChoiceQuestion),
                OpenTextQuestion openTextQuestion => _openTextDTOConverter.GenerateParameters(openTextQuestion),
                _ => null
            };

            if (generatedParameters == null)
            {
                return null;
            }
            generatedQuestions.Add(new QuestionResponseDTO
            {
                QuestionText = question.Text,
                Id = question.Id,
                QuestionType = QuestionTypeConverter.ToString(question.Type),
                QuestionParameters = generatedParameters
            });
        }

        return generatedQuestions;
    }

    public IEnumerable<QuizResponseDTO> GetQuizzes()
    {
        var quizzes = _quizRepository.GetQuizzes();

        return quizzes.Select(quiz => new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id });
    }

    public QuizResponseDTO? GetQuiz(int id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return null;
        }

        return new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id };
    }

    public async Task<AnswerSubmitResponseDTO> SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request)
    {
        var response = new AnswerSubmitResponseDTO();
        var quiz = _quizRepository.GetQuizById(id);
        var correctAnswers = 0;

        if (quiz == null)
        {
            response.Status = "failed";
            return response;
        }
        else
        {
            response.Status = "success";
        }
        
        var explanations = new List<ExplanationDTO>();
        bool includeExplanations = true;
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
                    if (answer.OptionName != null && SingleChoiceAnswerChecker.IsCorrect(singleChoiceQuestion, answer.OptionName))
                    {
                        correctAnswers++;
                        correctExplanationAnswer = true;
                    }
                    break;

                case MultipleChoiceQuestion multipleChoiceQuestion:
                    if (answer.OptionNames != null)
                    {
                        if (MultipleChoiceAnswerChecker.IsCorrect(multipleChoiceQuestion, answer.OptionNames.Select(opt => new Option { Name = opt }).ToList()))
                        {
                            correctAnswers++;
                            correctExplanationAnswer = true;
                        }
                    }
                    break;

                case OpenTextQuestion openTextQuestion:
                    var answerText = answer.AnswerText;
                    if (answerText != null && OpenTextAnswerChecker.IsCorrect(openTextQuestion, answerText, trimWhitespace: true))
                    {
                        correctAnswers++;
                        correctExplanationAnswer = true;
                    }
                    break;
            }
            
            if (includeExplanations)
            {
                switch (question)
                {
                    case SingleChoiceQuestion singleChoiceQuestion:
                        var explanationSingleChoice = await _explanationService.GenerateExplanationAsync(singleChoiceQuestion, answer.OptionName ?? answer.AnswerText, correctExplanationAnswer);
                        if (!string.IsNullOrEmpty(explanationSingleChoice) && !explanationSingleChoice.Contains("Explanation generation failed."))
                        {
                            explanations.Add(new ExplanationDTO { QuestionId = question.Id, Explanation = explanationSingleChoice, Correct = correctExplanationAnswer });
                        }
                        break;

                    case MultipleChoiceQuestion multipleChoiceQuestion:
                        var explanationMultipleChoice = await _explanationService.GenerateExplanationAsync(multipleChoiceQuestion, answer.OptionNames ?? new List<string>(), correctExplanationAnswer);
                        if (!string.IsNullOrEmpty(explanationMultipleChoice) && !explanationMultipleChoice.Contains("Explanation generation failed."))
                        {
                            explanations.Add(new ExplanationDTO { QuestionId = question.Id, Explanation = explanationMultipleChoice, Correct = correctExplanationAnswer});
                        }
                        break;

                    case OpenTextQuestion openTextQuestion:
                        var explanationOpenText = await _explanationService.GenerateExplanationAsync(openTextQuestion, answer.AnswerText ?? "", correctExplanationAnswer);
                        if (!string.IsNullOrEmpty(explanationOpenText) && !explanationOpenText.Contains("Explanation generation failed."))
                        {
                            explanations.Add(new ExplanationDTO { QuestionId = question.Id, Explanation = explanationOpenText, Correct = correctExplanationAnswer});
                        }
                        break;
                }
            }
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
}