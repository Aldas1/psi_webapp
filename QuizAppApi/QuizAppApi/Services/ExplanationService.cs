using QuizAppApi.Interfaces;
using OpenAI_API;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

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

    private string? GenerateQuestionExplanationQuery(string question, string options, string type)
    {
        return $"Question: {question}\nOptions: {options}\nExplain this {type} answer (keep the explanation short):";
    }

    private string? GenerateCommentExplanationQuery(string userComment)
    {
        return "You are a helpful assistant. Keep your answers short. This is the user message: " + userComment;
    }

    public async Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question)
    {
        return await GenerateAnswer(GenerateQuestionExplanationQuery(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), QuestionTypeConverter.ToReadableString(question.Type)));
    }

    public async Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question)
    {
        return await GenerateAnswer(GenerateQuestionExplanationQuery(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), QuestionTypeConverter.ToReadableString(question.Type)));
    }

    public async Task<string?> GenerateExplanationAsync(OpenTextQuestion question)
    {
        return await GenerateAnswer(GenerateQuestionExplanationQuery(question.Text, "", QuestionTypeConverter.ToReadableString(question.Type)));
    }

    public async Task<string?> GenerateCommentExplanationAsync(string? userComment)
    {
        return await GenerateAnswer(GenerateCommentExplanationQuery(userComment: userComment));
    }
}