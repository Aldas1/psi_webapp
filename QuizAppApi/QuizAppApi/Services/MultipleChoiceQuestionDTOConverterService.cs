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

            if (options == null || correctOptionIndexes == null || correctOptionIndexes.Min() < 0 ||
                correctOptionIndexes.Max() >= options.Count)
            {
                return null;
            }

            var generatedOptions = options.Select(o => new Option { Name = o }).ToList();
            foreach (var i in correctOptionIndexes)
            {
                generatedOptions[i].Correct = true;
            }

            return new MultipleChoiceQuestion
            {
                Options = generatedOptions
            };
        }

        public QuestionParametersDTO GenerateParameters(MultipleChoiceQuestion question)
        {
            return new QuestionParametersDTO
            {
                Options = question.Options.Select(opt => opt.Name).ToList(),
                CorrectOptionIndexes =
                        question.Options.Select((option, index) => new { Option = option, Index = index })
                        .Where(o => o.Option.Correct)
                        .Select(o => o.Index)
                        .ToList()
            };
        }
    }
}