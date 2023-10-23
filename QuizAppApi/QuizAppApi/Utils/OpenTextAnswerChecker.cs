using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils;

public static class OpenTextAnswerChecker
{
    public static bool IsCorrect(OpenTextQuestion question, string answerPara, bool useLowercaseComparison = false, bool trimWhitespace = false)
    {
        var answer = trimWhitespace ? answerPara.Trim() : answerPara;
        var correctAnswer = trimWhitespace ? question.CorrectAnswer.Trim() : question.CorrectAnswer;
            
        return useLowercaseComparison ? answer.ToLower() == correctAnswer.ToLower() : answer == correctAnswer;
    }
}