using System.Text.RegularExpressions;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services;

public class OpenTextQuestionDTOConverterService : IQuestionDTOConverterService<OpenTextQuestion>
{
    public OpenTextQuestion CreateFromParameters(QuestionParametersDTO questionDTO)
    {
        var correctText = questionDTO.CorrectText;

        if (correctText == null)
        {
            throw new DTOConversionException("Open text question: no correct text provided");
        }

        if (!Regex.IsMatch(correctText, @"^[a-zA-Z0-9 .,!?()-]+$"))
        {
            throw new DTOConversionException("Open text question only accepts letters, numbers and some symbols");
        }
        

        return new OpenTextQuestion { CorrectAnswer = correctText };
    }

    public QuestionParametersDTO GenerateParameters(OpenTextQuestion question)
    {
        return new QuestionParametersDTO { CorrectText = question.CorrectAnswer };
    }
}
