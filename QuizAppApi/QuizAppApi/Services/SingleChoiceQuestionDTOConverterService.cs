using QuizAppApi.Dtos;
using QuizAppApi.Exceptions;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services;

public class SingleChoiceQuestionDtoConverterService : IQuestionDtoConverterService<SingleChoiceQuestion>
{
    public SingleChoiceQuestion CreateFromParameters(QuestionParametersDto questionDto)
    {
        var options = questionDto.Options;
        var correctOptionIndex = questionDto.CorrectOptionIndex;

        if (options == null || correctOptionIndex == null || correctOptionIndex < 0 ||
            correctOptionIndex >= options.Count)
        {
            throw new DtoConversionException("Client did not provide required info");
        }

        if (options.Distinct().Count() != options.Count)
        {
            throw new DtoConversionException("Duplicate options are not allowed");
        }

        var correctOptionName = options[(int)correctOptionIndex];

        return new SingleChoiceQuestion
        {
            Options = options.Select(o => new Option { Name = o, Correct = o == correctOptionName }).ToList(),
        };
    }

    public QuestionParametersDto GenerateParameters(SingleChoiceQuestion question)
    {
        return new QuestionParametersDto
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