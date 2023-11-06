using QuizAppApi.Dtos;
using QuizAppApi.Models;

namespace QuizAppApi.Interfaces;

public interface IQuestionDtoConverterService<TQuestion> where TQuestion : Question
{
    TQuestion CreateFromParameters(QuestionParametersDto questionDto);
    QuestionParametersDto GenerateParameters(TQuestion question);
}