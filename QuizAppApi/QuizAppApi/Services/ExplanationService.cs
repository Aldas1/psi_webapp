using QuizAppApi.Interfaces;
using OpenAI_API;
using QuizAppApi.Dtos;
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
    
    private async Task<string?> AnswerGeneration(string prompt)
    {
        try
        {
            var openAi = new OpenAIAPI(_openAiApiKey);
            
            var responses = await openAi.Completions.CreateCompletionAsync(
                model: "gpt-3.5-turbo-instruct",
                prompt: prompt,
                max_tokens: 150
            );

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
        return $"Question: {question}\n{options}\nExplain this {type} answer (keep the explanation short):";
    }
    public async Task<string?> GenerateExplanationAsync(QuestionResponseDto question)
    {
        var options = question.QuestionParameters?.Options != null
            ? "Options: " + string.Join(", ", question.QuestionParameters.Options.Select(opt => opt))
            : "";

        return await AnswerGeneration(GenerateQuestionExplanationQuery(question.QuestionText, options, "question"));
    }

    public async Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question)
    {
        return await AnswerGeneration(GenerateQuestionExplanationQuery(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), "singleChoiceQuestion"));
    }

    public async Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question)
    {
        return await AnswerGeneration(GenerateQuestionExplanationQuery(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), "multipleChoiceQuestion"));
    }

    public async Task<string?> GenerateExplanationAsync(OpenTextQuestion question)
    {
        return await AnswerGeneration(GenerateQuestionExplanationQuery(question.Text, "", "openTextQuestion"));
    }
    
    private string? GenerateCommentExplanationQuery(string userComment)
    {
        return "You are a helpful assistant. Keep your answers short. This is the user message: " + userComment;
    }
    
    public async Task<string?> GenerateCommentExplanationAsync(string? userComment)
    {
        return await AnswerGeneration(GenerateCommentExplanationQuery(userComment: userComment));
    }
}