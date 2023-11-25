using QuizAppApi.Interfaces;
using OpenAI_API;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;
using OpenAI_API.Completions;
using Microsoft.Identity.Client;

namespace QuizAppApi.Services;
public class ExplanationService : IExplanationService
{
    private readonly string _openAiApiKey;
    private readonly OpenAIAPI? openAi;

    public ExplanationService(string? openAiApiKey)
    {
        _openAiApiKey = openAiApiKey ?? throw new ArgumentNullException(nameof(openAiApiKey));
        openAi = new OpenAIAPI(_openAiApiKey);
        openAi.Completions.DefaultCompletionRequestArgs.Model = "gpt-3.5-turbo-instruct";
        openAi.Completions.DefaultCompletionRequestArgs.MaxTokens = 150;
    }

    private async Task<string?> GenerateAnswer(string prompt)
    {
        try
        {            
            var responses = await openAi.Completions.CreateCompletionAsync(prompt);
            
            if (responses?.Completions == null)
            {
                return null;
            }

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

    private string? FormatQuery(string? question = null, string? options = null, string? answer = null, string? type = null, string? userComment = null)
    {
        string query;
        if (userComment == null)
        {
            query = $"Question: {question}\n{options}\nAnswer: {answer}\nExplain this {type} answer (keep the explanation short):";
        }
        else
        {
            query = "You are a helpful assistant. Keep your answers short. This is the user message: " + userComment;
        }
        return query;
    }

    public async Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question, string? chosenOption)
    {
        return await GenerateAnswer(FormatQuery(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), chosenOption, QuestionTypeConverter.ToReadableString(question.Type)));
    }

    public async Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question, List<string> chosenOptions)
    {
        return await GenerateAnswer(FormatQuery(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), string.Join(", ", chosenOptions), QuestionTypeConverter.ToReadableString(question.Type)));
    }

    public async Task<string?> GenerateExplanationAsync(OpenTextQuestion question, string? answerText)
    {
        return await GenerateAnswer(FormatQuery(question.Text, "", answerText, QuestionTypeConverter.ToReadableString(question.Type)));
    }

    public async Task<string?> GenerateCommentExplanationAsync(string? userComment)
    {
        return await GenerateAnswer(FormatQuery(userComment: userComment));
    }
}