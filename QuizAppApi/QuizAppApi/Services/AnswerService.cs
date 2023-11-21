using QuizAppApi.Dtos;
using QuizAppApi.Events;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services;

public class AnswerService : IAnswerService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IAnswerCheckerService _answerCheckerService;

    public AnswerService(IQuizRepository quizRepository, IAnswerCheckerService answerCheckerService)
    {
        _quizRepository = quizRepository;
        _answerCheckerService = answerCheckerService;
    }

    public async Task<AnswerSubmitResponseDto> SubmitAnswersAsync(int id, List<AnswerSubmitRequestDto> request, string? username)
    {
        var response = new AnswerSubmitResponseDto();
        var quiz = await _quizRepository.GetQuizByIdAsync(id);
        int correctAnswers = 0;

        if (quiz == null)
        {
            response.Status = "failed";
            return response;
        }

        response.Status = "success";

        foreach (var answer in request)
        {
            var question = quiz.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question == null)
            {
                continue;
            }

            correctAnswers += IsAnswerCorrect(question, answer) ? 1 : 0;
        }

        response.CorrectlyAnswered = correctAnswers;
        response.Score = quiz.Questions.Count == 0 ? 0 : ((double)correctAnswers * 100 / quiz.Questions.Count);

        AnswerSubmittedEvent.Raise(this, new AnswerSubmittedEventArgs
        {
            QuizId = quiz.Id,
            Score = response.Score,
            Username = username
        });

        return response;
    }

    private bool IsAnswerCorrect(Question question, AnswerSubmitRequestDto answerRequest)
    {
        switch (question)
        {
            case SingleChoiceQuestion singleChoiceQuestion:
                if (answerRequest.OptionName != null &&
                    _answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, answerRequest.OptionName))
                {
                    return true;
                }

                break;

            case MultipleChoiceQuestion multipleChoiceQuestion:
                if (answerRequest.OptionNames != null)
                {
                    if (_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion,
                            answerRequest.OptionNames.Select(optName => new Option { Name = optName }).ToList()))
                    {
                        return true;
                    }
                }

                break;

            case OpenTextQuestion openTextQuestion:
                var answerText = answerRequest.AnswerText;
                if (answerText != null &&
                    _answerCheckerService.CheckOpenTextAnswer(openTextQuestion, answerText, trimWhitespace: true, useLowercaseComparison: true))
                {
                    return true;
                }

                break;
        }

        return false;
    }
}