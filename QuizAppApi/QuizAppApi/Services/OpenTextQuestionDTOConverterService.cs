using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services
{
    public class OpenTextQuestionDTOConverterService : IQuestionDTOConverterService<OpenTextQuestion>
    {
        public OpenTextQuestion? CreateFromParameters(QuestionParametersDTO questionDTO)
        {
            var correctText = questionDTO.CorrectText;

            if (correctText == null)
            {
                return null;
            }

            return new OpenTextQuestion { CorrectAnswer = correctText };
        }

        public QuestionParametersDTO GenerateParameters(OpenTextQuestion question)
        {
            return new QuestionParametersDTO { CorrectText = question.CorrectAnswer };
        }
    }
}
