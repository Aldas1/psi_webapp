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
    
    private async Task<string?> AnswerGeneration(string question, string options, string type)
    {
        try
        {
            var openAi = new OpenAIAPI(_openAiApiKey);

            var prompt = $"Question: {question}\n{options}\nExplain this {type} answer (keep the explanation short):";
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
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<string?> GenerateExplanationAsync(QuestionResponseDto question)
    {
        var options = question.QuestionParameters?.Options != null
            ? "Options: " + string.Join(", ", question.QuestionParameters.Options.Select(opt => opt))
            : "";

        return await AnswerGeneration(question.QuestionText, options, "question");
    }

    public async Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question)
    {
        return await AnswerGeneration(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), "singleChoiceQuestion");
    }

    public async Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question)
    {
        return await AnswerGeneration(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), "multipleChoiceQuestion");
    }

    public async Task<string?> GenerateExplanationAsync(OpenTextQuestion question)
    {
        return await AnswerGeneration(question.Text, "", "openTextQuestion");
    }
}