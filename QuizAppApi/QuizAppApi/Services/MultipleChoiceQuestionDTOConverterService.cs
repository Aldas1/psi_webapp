using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services
{
    public class MultipleChoiceQuestionDTOConverterService : IQuestionDTOConverterService<MultipleChoiceQuestion>
    {
        public MultipleChoiceQuestion? CreateFromParameters(QuestionParametersDTO questionDTO)
        {
            var options = questionDTO.Options;
            var correctOptionIndexes = questionDTO.CorrectOptionIndexes;

            if (options == null || correctOptionIndexes == null || correctOptionIndexes.Min() < 0 || correctOptionIndexes.Max() >= options.Count)
            {
                return null;
            }

            return new MultipleChoiceQuestion
            {
                Options = options.Select(o => new Option { Name = o }).ToList(),
                CorrectOptions = correctOptionIndexes.Select(i => new Option { Name = options[(int)i] }).ToList()
            };
        }

        public QuestionParametersDTO GenerateParameters(MultipleChoiceQuestion question)
        {
            return new QuestionParametersDTO { Options = question.Options.Select(opt => opt.Name).ToList() };
        }
    }
}
