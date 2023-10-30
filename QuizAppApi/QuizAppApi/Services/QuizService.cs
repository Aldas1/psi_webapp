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
    private readonly IQuestionDTOConverterService<SingleChoiceQuestion> _singleChoiceDTOConverter;
    private readonly IQuestionDTOConverterService<MultipleChoiceQuestion> _multipleChoiceDTOConverter;
    private readonly IQuestionDTOConverterService<OpenTextQuestion> _openTextDTOConverter;
    private readonly EventPublisher _eventPublisher;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public QuizService(
        IQuizRepository quizRepository,
        IQuestionDTOConverterService<SingleChoiceQuestion> singleChoiceDTOConverter,
        IQuestionDTOConverterService<MultipleChoiceQuestion> multipleChoiceDTOConverter,
        IQuestionDTOConverterService<OpenTextQuestion> openTextDTOConverter,
        EventPublisher eventPublisher,
        IServiceScopeFactory serviceScopeFactory)
    {
        _quizRepository = quizRepository;
        _singleChoiceDTOConverter = singleChoiceDTOConverter;
        _multipleChoiceDTOConverter = multipleChoiceDTOConverter;
        _openTextDTOConverter = openTextDTOConverter;
        _eventPublisher = eventPublisher;
        _serviceScopeFactory = serviceScopeFactory;

        _eventPublisher.AnswerSubmitted += (sender, e) =>
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedQuizRepository = scope.ServiceProvider.GetRequiredService<IQuizRepository>();
                Console.WriteLine("udpating stuff");
                var quiz = scopedQuizRepository.GetQuizById(e.QuizId);
                if (quiz == null) return;
                quiz.NumberOfSubmitters++;
                scopedQuizRepository.Save();
            }
        };
    }

    public QuizManipulationResponseDTO CreateQuiz(QuizManipulationRequestDTO request)
    {
        var newQuiz = new Quiz();

        try
        {
            PopulateQuizFromDTO(newQuiz, request);
        }
        catch (DTOConversionException e)
        {
            return new QuizManipulationResponseDTO { Status = e.Message };
        }

        Quiz? createdQuiz = _quizRepository.AddQuiz(newQuiz);

        if (createdQuiz == null)
        {
            return new QuizManipulationResponseDTO { Status = "failed" };
        }

        int createdQuizId = createdQuiz.Id;

        return new QuizManipulationResponseDTO { Status = "success", Id = createdQuizId };
    }

    public QuizManipulationResponseDTO UpdateQuiz(int id, QuizManipulationRequestDTO editRequest)
    {
        var newQuiz = _quizRepository.GetQuizById(id);
        if (newQuiz == null)
        {
            return new QuizManipulationResponseDTO { Status = "Quiz not found" };
        }

        try
        {
            PopulateQuizFromDTO(newQuiz, editRequest);
        }
        catch (DTOConversionException e)
        {
            return new QuizManipulationResponseDTO { Status = e.Message };
        }

        _quizRepository.Save();
        return new QuizManipulationResponseDTO { Status = "success", Id = id };
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

        return quizzes.Select(quiz => new QuizResponseDTO
            { Name = quiz.Name, Id = quiz.Id, NumberOfSubmitters = quiz.NumberOfSubmitters });
    }

    public QuizResponseDTO? GetQuiz(int id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return null;
        }

        return new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id, NumberOfSubmitters = quiz.NumberOfSubmitters };
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

    private void PopulateQuizFromDTO(Quiz quiz, QuizManipulationRequestDTO request)
    {
        quiz.Name = request.Name;
        quiz.Questions.Clear();

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
                throw new DTOConversionException("Failed to create a question");
            }

            generatedQuestion.Text = question.QuestionText;
            quiz.Questions.Add(generatedQuestion);
        }
    }
}