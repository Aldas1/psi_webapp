using QuizAppApi.DTOs;
using QuizAppApi.Models;

namespace QuizAppApi.Interfaces
{
    public interface IQuestionDTOConverterService<TQuestion> where TQuestion : Question
    {
        TQuestion? CreateFromParameters(QuestionParametersDTO questionDTO);
        QuestionParametersDTO GenerateParameters(TQuestion question);
    }
}
