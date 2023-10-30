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

        return new OpenTextQuestion { CorrectAnswer = correctText };
    }

    public QuestionParametersDTO GenerateParameters(OpenTextQuestion question)
    {
        return new QuestionParametersDTO { CorrectText = question.CorrectAnswer };
    }
}
