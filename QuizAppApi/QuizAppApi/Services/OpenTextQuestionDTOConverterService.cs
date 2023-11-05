using System.Text.RegularExpressions;
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

        if (!Regex.IsMatch(correctText, @"^[a-zA-Z0-9 .,!?()-]+$"))
        {
            throw new DtoConversionException("Open text question only accepts letters, numbers and some symbols");
        }
        

        return new OpenTextQuestion { CorrectAnswer = correctText };
    }

    public QuestionParametersDto GenerateParameters(OpenTextQuestion question)
    {
        return new QuestionParametersDto { CorrectText = question.CorrectAnswer };
    }
}
