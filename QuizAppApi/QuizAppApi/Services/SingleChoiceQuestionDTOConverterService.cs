using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services
{
    public class SingleChoiceQuestionDTOConverterService : IQuestionDTOConverterService<SingleChoiceQuestion>
    {
        public SingleChoiceQuestion? CreateFromParameters(QuestionParametersDTO questionDTO)
        {
            var options = questionDTO.Options;
            if (options == null && options.Distinct().Count() == options.Count)
            {
                return null;
            }

            var correctOptionIndex = questionDTO.CorrectOptionIndex;

            if (correctOptionIndex == null || correctOptionIndex < 0 ||
                correctOptionIndex >= options.Count)
            {
                return null;
            }

            var correctOptionName = options[(int)correctOptionIndex];

            return new SingleChoiceQuestion
            {
                Options = options.Select(o => new Option { Name = o, Correct = o == correctOptionName }).ToList(),
            };
        }

        public QuestionParametersDTO GenerateParameters(SingleChoiceQuestion question)
        {
            return new QuestionParametersDTO
                { Options = question.Options.Select(opt => opt.Name).ToList() };
        }
    }
}