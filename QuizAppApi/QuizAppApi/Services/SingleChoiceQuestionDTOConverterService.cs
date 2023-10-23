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
            var correctOptionIndex = questionDTO.CorrectOptionIndex;

            if (options == null || correctOptionIndex == null || correctOptionIndex < 0 ||
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
            {
                Options = question.Options.Select(opt => opt.Name).ToList(),
                CorrectOptionIndex =
            question.Options.Select((option, index) => new { Option = option, Index = index })
            .Where(o => o.Option.Correct)
            .Select(o => o.Index)
            .ToList()
            .First()
            };
        }
    }
}