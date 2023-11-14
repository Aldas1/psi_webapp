using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

namespace QuizAppApi.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionDtoConverterService<SingleChoiceQuestion> _singleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<MultipleChoiceQuestion> _multipleChoiceDtoConverter;
    private readonly IQuestionDtoConverterService<OpenTextQuestion> _openTextDtoConverter;

    public QuestionService(
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

    public async Task<IEnumerable<QuestionResponseDto>?> GetQuestions(int id)
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
}