using System;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using OpenAI_API;

namespace QuizAppApi.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly string _openAiApiKey;

        public ChatGptService(string openAiApiKey)
        {
            _openAiApiKey = openAiApiKey ?? throw new ArgumentNullException(nameof(openAiApiKey));
        }

        public string GenerateExplanation(string questionText, string chosenOption)
        {
            try
            {
                var openAi = new OpenAIAPI(_openAiApiKey);

                var prompt = $"Question: {questionText}\nAnswer: {chosenOption}\nExplanation:";
                var responses = openAi.Completions.CreateCompletionAsync(
                    model: "gpt-3.5-turbo-instruct",
                    prompt: prompt,
                    max_tokens: 50
                );

                if (responses == null) return "Explanation generation failed.";

                var explanationResponses = new List<string>();

                foreach (var response in responses.Result.Completions)
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