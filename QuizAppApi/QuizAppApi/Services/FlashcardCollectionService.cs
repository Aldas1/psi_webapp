using QuizAppApi.Dtos.Flashcards;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Flashcards;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services;

public class FlashcardCollectionService : IFlashcardCollectionService
{
    private readonly IFlashcardCollectionRepository _repository;
    private readonly IFlashcardService _flashcardService;
    private readonly IQuizRepository _quizRepository;

    public FlashcardCollectionService(
        IFlashcardCollectionRepository flashcardCollectionRepository,
        IFlashcardService flashcardService,
        IQuizRepository quizRepository)
    {
        _repository = flashcardCollectionRepository;
        _flashcardService = flashcardService;
        _quizRepository = quizRepository;
    }

    public async Task<FlashcardCollectionDto> CreateAsync(FlashcardCollectionDto collectionDto)
    {
        return ToDto(await _repository.CreateAsync(new FlashcardCollection { Name = collectionDto.Name }));
    }

    public async Task<IEnumerable<FlashcardCollectionDto>> CreateFromQuizzesAsync(int quizId)
    {

        var quiz = await _quizRepository.GetQuizByIdAsync(quizId);
        if (quiz == null)
        {
            return null;
        }

        var result = await _repository.CreateAsync(new FlashcardCollection { Name = quiz.Name + " flashcards"});
        foreach (var question in quiz.Questions)
        {
            string? answer;
            switch (question)
            {
                case SingleChoiceQuestion singleChoiceQuestion:
                    answer = singleChoiceQuestion.Options
                        .Where(option => option.Correct)
                        .Select(option => option.Name)
                        .FirstOrDefault();
                    break;

                case MultipleChoiceQuestion multipleChoiceQuestion:
                    var correctOptions = multipleChoiceQuestion.Options
                        .Where(option => option.Correct)
                        .Select(option => option.Name)
                        .ToList();
                    answer = string.Join(", ", correctOptions);
                    break;

                case OpenTextQuestion openTextQuestion:
                    answer = openTextQuestion.CorrectAnswer;
                    break;

                default:
                    answer = "";
                    break;
            }

            await _flashcardService.CreateAsync(result.Id, new FlashcardDto
            {
                Question = question.Text,
                Answer = answer
            });
        }
        return await GetAsync();
    }

    public async Task<FlashcardCollectionDto?> GetByIdAsync(int id)
    {
        var res = await _repository.GetByIdAsync(id);
        return res == null ? null : ToDto(res);
    }

    public async Task<IEnumerable<FlashcardCollectionDto>> GetAsync()
    {
        return (await _repository.GetAsync()).Select(ToDto);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static FlashcardCollectionDto ToDto(FlashcardCollection collection)
    {
        return new FlashcardCollectionDto
        {
            Id = collection.Id,
            Name = collection.Name
        };
    }
}