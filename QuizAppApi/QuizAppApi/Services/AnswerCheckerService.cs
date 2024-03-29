using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;


namespace QuizAppApi.Services;

public class AnswerCheckerService : IAnswerCheckerService
{
    public bool CheckSingleChoiceAnswer(SingleChoiceQuestion question, string answer)
    {
        foreach (var option in question.Options)
        {
            if (option.Correct && option.Name == answer)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckMultipleChoiceAnswer(MultipleChoiceQuestion question, ICollection<Option> answer)
    {
        var correctAnswers = new HashSet<Option>(question.Options.Where(opt => opt.Correct), new OptionEqualityComparer());
        var updatedAnswer = new HashSet<Option>(answer, new OptionEqualityComparer());

        return correctAnswers.SetEquals(updatedAnswer);
    }

    public bool CheckOpenTextAnswer(OpenTextQuestion question, string answerPara, bool useLowercaseComparison = false, bool trimWhitespace = false)
    {
        var answer = trimWhitespace ? answerPara.Trim() : answerPara;
        var correctAnswer = trimWhitespace ? question.CorrectAnswer.Trim() : question.CorrectAnswer;

        return useLowercaseComparison ? answer.ToLower() == correctAnswer.ToLower() : answer == correctAnswer;
    }
}