using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

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

    public AnswerSubmitResponseDto SubmitAnswers(int id, List<AnswerSubmitRequestDto> request)
    {
        var response = new AnswerSubmitResponseDto();
        var quiz = _quizRepository.GetQuizById(id);
        var correctAnswers = 0;
        
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
        response.Score = quiz.Questions.Count == 0 ? 0 : (correctAnswers * 100 / quiz.Questions.Count);
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
                    _answerCheckerService.CheckOpenTextAnswer(openTextQuestion, answerText, trimWhitespace: true))
                {
                    return true;
                }
        
                break;
        }

        return false;
    }
}