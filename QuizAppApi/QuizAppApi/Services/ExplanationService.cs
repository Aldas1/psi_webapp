using QuizAppApi.Interfaces;
using OpenAI_API;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

namespace QuizAppApi.Services;
public class ExplanationService : IExplanationService
{
    private readonly string _openAiApiKey;

    public ExplanationService(string? openAiApiKey)
    {
        _openAiApiKey = openAiApiKey ?? throw new ArgumentNullException(nameof(openAiApiKey));
    }

    private async Task<string?> AnswerGeneration(string question = null, string options = null, string? answer = null, string type = null, string userComment = null)
    {
        try
        {
            var openAi = new OpenAIAPI(_openAiApiKey);
            var prompt = "";
            if (userComment == null)
            {
                prompt = $"Question: {question}\n{options}\nAnswer: {answer}\nExplain this {type} answer (keep the explanation short):";
            } else
            {
                prompt = userComment;
            }
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
    
    public async Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question, string? chosenOption)
    {
        return await AnswerGeneration(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), chosenOption, QuestionTypeConverter.ToString(question.Type));
    }

    public async Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question, List<string> chosenOptions)
    {
        return await AnswerGeneration(question.Text, "Options: " + string.Join(", ", question.Options.Select(opt => opt.Name)), string.Join(", ", chosenOptions), QuestionTypeConverter.ToString(question.Type));
    }

    public async Task<string?> GenerateExplanationAsync(OpenTextQuestion question, string? answerText)
    {
        return await AnswerGeneration(question.Text, "", answerText, QuestionTypeConverter.ToString(question.Type));
    }

    public async Task<string?> GenerateCommentExplanationAsync(string explainQuery)
    {
        return await AnswerGeneration(userComment: explainQuery);
    }
}