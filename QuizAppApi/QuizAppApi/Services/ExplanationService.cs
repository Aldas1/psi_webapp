using System;
using Microsoft.Extensions.Options;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using OpenAI_API;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services
{
    public class ExplanationService : IExplanationService
    {
        private readonly string _openAiApiKey;

        public ExplanationService(string openAiApiKey)
        {
            _openAiApiKey = openAiApiKey ?? throw new ArgumentNullException(nameof(openAiApiKey));
        }
        
        public async Task<string> GenerateExplanationAsync(SingleChoiceQuestion question, string chosenOption)
        {
            try
            {
                var openAi = new OpenAIAPI(_openAiApiKey);

                var prompt = $"Question: {question.Text}\nAnswer: {chosenOption}\nExplain this answer (keep the explanation short):";
                var responses = await openAi.Completions.CreateCompletionAsync(
                    model: "gpt-3.5-turbo-instruct",
                    prompt: prompt,
                    max_tokens: 150
                );

                if (responses == null || responses.Completions == null)
                    return "Explanation generation failed.";

                var explanationResponses = new List<string>();

                foreach (var response in responses.Completions)
                {
                    explanationResponses.Add(response.Text.Trim());
                }

                return string.Join("\n", explanationResponses);
            }
            catch (Exception ex)
            {
                return $"Explanation generation error: {ex.Message}";
            }
        }

        public async Task<string> GenerateExplanationAsync(MultipleChoiceQuestion question, List<string> chosenOptions)
        {
            try
            {
                var openAi = new OpenAIAPI(_openAiApiKey);

                var prompt = $"Question: {question.Text}\nAnswer: {string.Join(", ", chosenOptions)}\nExplain this answer (keep the explanation short):";
                var responses = await openAi.Completions.CreateCompletionAsync(
                    model: "gpt-3.5-turbo-instruct",
                    prompt: prompt,
                    max_tokens: 150
                );

                if (responses == null || responses.Completions == null)
                    return "Explanation generation failed.";

                var explanationResponses = new List<string>();

                foreach (var response in responses.Completions)
                {
                    explanationResponses.Add(response.Text.Trim());
                }

                return string.Join("\n", explanationResponses);
            }
            catch (Exception ex)
            {
                return $"Explanation generation error: {ex.Message}";
            }
        }

        public async Task<string> GenerateExplanationAsync(OpenTextQuestion question, string answerText)
        {
            try
            {
                var openAi = new OpenAIAPI(_openAiApiKey);

                var prompt = $"Question: {question.Text}\nAnswer: {answerText}\nExplain this answer (keep the explanation short):";
                var responses = await openAi.Completions.CreateCompletionAsync(
                    model: "gpt-3.5-turbo-instruct",
                    prompt: prompt,
                    max_tokens: 150
                );

                if (responses == null || responses.Completions == null)
                    return "Explanation generation failed.";

                var explanationResponses = new List<string>();

                foreach (var response in responses.Completions)
                {
                    explanationResponses.Add(response.Text.Trim());
                }

                return string.Join("\n", explanationResponses);
            }
            catch (Exception ex)
            {
                return $"Explanation generation error: {ex.Message}";
            }
        }
    }
}