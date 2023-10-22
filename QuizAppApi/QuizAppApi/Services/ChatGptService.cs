using System;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using OpenAI_API;

namespace QuizAppApi.Services
{
    public class ChatGptService : IChatGptService
    {
        public async Task<string> GenerateExplanationAsync(string questionText, string chosenOption)
        {
            try
            {
                var openAi = new OpenAIAPI("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");

                var prompt = $"Question: {questionText}\nAnswer: {chosenOption}\nExplanation:";
                var responses = await openAi.Completions.CreateCompletionAsync(
                    model: "gpt-3.5-turbo-instruct",
                    prompt: prompt,
                    max_tokens: 50
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