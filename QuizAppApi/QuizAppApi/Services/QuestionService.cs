using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

namespace QuizAppApi.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionDtoConverterService<SingleChoiceQuestion> _singleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<MultipleChoiceQuestion> _multipleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<OpenTextQuestion> _openTextDtoConverter;
    private readonly IExplanationService _explanationService;

    public QuestionService(
        IQuizRepository quizRepository,
        IQuestionDtoConverterService<SingleChoiceQuestion> singleChoiceDtoConverter,
        IQuestionDtoConverterService<MultipleChoiceQuestion> multipleChoiceDtoConverter,
        IQuestionDtoConverterService<OpenTextQuestion> openTextDtoConverter,
        IExplanationService explanationService)
    {
        _quizRepository = quizRepository;
        _singleChoiceDtoConverter = singleChoiceDtoConverter;
        _multipleChoiceDtoConverter = multipleChoiceDtoConverter;
        _openTextDtoConverter = openTextDtoConverter;
        _explanationService = explanationService;
    }

    public async Task<IEnumerable<QuestionResponseDto>?> GetQuestionsAsync(int id)
    {
        var quiz = await _quizRepository.GetQuizByIdAsync(id);
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

    public async Task<ExplanationDto?> GetQuestionExplanationAsync(int quizId, int questionId)
    {
        string? explanation;

        var quiz = await _quizRepository.GetQuizByIdAsync(quizId);
        if (quiz == null)
        {
            return null;
        }

        var question = quiz.Questions.FirstOrDefault(q => q.Id == questionId);
        if (question == null)
        {
            return null;
        }

        switch (question)
        {
            case SingleChoiceQuestion singleChoiceQuestion:
                explanation = await _explanationService.GenerateExplanationAsync(singleChoiceQuestion);
                break;
            case MultipleChoiceQuestion multipleChoiceQuestion:
                explanation = await _explanationService.GenerateExplanationAsync(multipleChoiceQuestion);
                break;
            case OpenTextQuestion openTextQuestion:
                explanation = await _explanationService.GenerateExplanationAsync(openTextQuestion);
                break;
            default:
                explanation = null;
                break;
        }
        
        return new ExplanationDto
        {
            Explanation = explanation
        };
    }
}