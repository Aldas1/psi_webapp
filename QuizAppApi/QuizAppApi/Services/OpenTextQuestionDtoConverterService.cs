using QuizAppApi.Dtos;
using QuizAppApi.Exceptions;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services;

public class OpenTextQuestionDtoConverterService : IQuestionDtoConverterService<OpenTextQuestion>
{
    public OpenTextQuestion CreateFromParameters(QuestionParametersDto questionDto)
    {
        var correctText = questionDto.CorrectText;

        if (correctText == null)
        {
            throw new DtoConversionException("Open text question: no correct text provided");
        }

        return new OpenTextQuestion { CorrectAnswer = correctText };
    }

    public QuestionParametersDto GenerateParameters(OpenTextQuestion question)
    {
        return new QuestionParametersDto { CorrectText = question.CorrectAnswer };
    }
}
