using QuizAppApi.Dtos;
using QuizAppApi.Exceptions;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services;

public class MultipleChoiceQuestionDtoConverterService : IQuestionDtoConverterService<MultipleChoiceQuestion>
{
    public MultipleChoiceQuestion CreateFromParameters(QuestionParametersDto questionDto)
    {
        var options = questionDto.Options;
        var correctOptionIndexes = questionDto.CorrectOptionIndexes;

        if (options == null || correctOptionIndexes == null || correctOptionIndexes.Min() < 0 ||
            correctOptionIndexes.Max() >= options.Count)
        {
            throw new DtoConversionException("Duplicate options are not allowed");
        }
        
        if (options.Distinct().Count() != options.Count)
        {
            throw new DtoConversionException("Duplicate options are not allowed");
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

    public QuestionParametersDto GenerateParameters(MultipleChoiceQuestion question)
    {
        return new QuestionParametersDto
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