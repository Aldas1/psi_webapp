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

    public async Task<QuizManipulationResponseDto> CreateQuizAsync(QuizManipulationRequestDto request,
        User? user = null)
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

        newQuiz.Username = user?.Username;

        Quiz? createdQuiz = await _quizRepository.AddQuizAsync(newQuiz);

        if (createdQuiz == null)
        {
            return new QuizManipulationResponseDto { Status = "failed" };
        }

        int createdQuizId = createdQuiz.Id;

        return new QuizManipulationResponseDto { Status = "success", Id = createdQuizId };
    }

    public async Task<QuizManipulationResponseDto> UpdateQuizAsync(int id, QuizManipulationRequestDto editRequest)
    {
        var newQuiz = await _quizRepository.GetQuizByIdAsync(id);
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

        await _quizRepository.SaveAsync();
        return new QuizManipulationResponseDto { Status = "success", Id = id };
    }

    public async Task<IEnumerable<QuizResponseDto>> GetQuizzesAsync()
    {
        var quizzes = await _quizRepository.GetQuizzesAsync();

        return quizzes.Select(quiz => new QuizResponseDto
            { Name = quiz.Name, Id = quiz.Id, NumberOfSubmitters = quiz.NumberOfSubmitters, Owner = quiz.Username });
    }

    public async Task<QuizResponseDto?> GetQuizAsync(int id)
    {
        var quiz = await _quizRepository.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return null;
        }

        return new QuizResponseDto
            { Name = quiz.Name, Id = quiz.Id, NumberOfSubmitters = quiz.NumberOfSubmitters, Owner = quiz.Username };
    }


    public async Task<bool> DeleteQuizAsync(int id)
    {
        var quiz = await _quizRepository.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return false;
        }

        await _quizRepository.DeleteQuizAsync(id);
        return true;
    }

    public async Task<bool> CanUserEditQuizAsync(User? user, int quizId)
    {
        var quiz = await _quizRepository.GetQuizByIdAsync(quizId);
        return quiz?.Username == null || quiz.Username == user?.Username;
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