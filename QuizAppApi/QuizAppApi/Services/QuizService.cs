using QuizAppApi.Dtos;
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
    private readonly IQuestionDtoConverterService<SingleChoiceQuestion> _singleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<MultipleChoiceQuestion> _multipleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<OpenTextQuestion> _openTextDtoConverter;

    public QuizService(
        IQuizRepository quizRepository,
        IQuestionDtoConverterService<SingleChoiceQuestion> singleChoiceDtoConverter,
        IQuestionDtoConverterService<MultipleChoiceQuestion> multipleChoiceDtoConverter,
        IQuestionDtoConverterService<OpenTextQuestion> openTextDtoConverter)
    {
        _quizRepository = quizRepository;
        _singleChoiceDtoConverter = singleChoiceDtoConverter;
        _multipleChoiceDtoConverter = multipleChoiceDtoConverter;
        _openTextDtoConverter = openTextDtoConverter;
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

    public IEnumerable<QuizResponseDto> GetQuizzes()
    {
        var quizzes = _quizRepository.GetQuizzes();

        return quizzes.Select(quiz => new QuizResponseDto
            { Name = quiz.Name, Id = quiz.Id, NumberOfSubmitters = quiz.NumberOfSubmitters });
    }

    public QuizResponseDto? GetQuiz(int id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return null;
        }

        return new QuizResponseDto { Name = quiz.Name, Id = quiz.Id, NumberOfSubmitters = quiz.NumberOfSubmitters };
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