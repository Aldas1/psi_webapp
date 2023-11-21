using System;
using Microsoft.Extensions.Options;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using OpenAI_API;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services;
public class ExplanationService : IExplanationService
{
    private readonly string _openAiApiKey;

    public ExplanationService(string? openAiApiKey)
    {
        _openAiApiKey = openAiApiKey ?? throw new ArgumentNullException(nameof(openAiApiKey));
    }

    private async Task<string?> AnswerGeneration(string question, string options, string? answer, string type)
    {
        try
        {
            var openAi = new OpenAIAPI(_openAiApiKey);

            var prompt = $"Question: {question}\n{options}\nAnswer: {answer}\nExplain this {type} answer (keep the explanation short):";
            var responses = await openAi.Completions.CreateCompletionAsync(
                model: "gpt-3.5-turbo-instruct",
                prompt: prompt,
                max_tokens: 150
            );

            if (responses?.Completions == null)
                return null;

            var explanationResponses = new List<string>();

            foreach (var response in responses.Completions)
            {
                explanationResponses.Add(response.Text.Trim());
            }

            return string.Join("\n", explanationResponses);
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    public async Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question, string? chosenOption)
    {
        return await AnswerGeneration(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), chosenOption, "single choice question");
    }

    public async Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question, List<string> chosenOptions)
    {
        return await AnswerGeneration(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), string.Join(", ", chosenOptions), "multiple choice question");
    }


    public async Task<string?> GenerateExplanationAsync(OpenTextQuestion question, string? answerText)
    {
        return await AnswerGeneration(question.Text, "", answerText, "open text question");
    }
}