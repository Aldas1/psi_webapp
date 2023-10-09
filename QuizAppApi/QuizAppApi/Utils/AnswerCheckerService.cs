using QuizAppApi.Models.Questions;
using System.Collections.Generic;

namespace QuizAppApi.Services
{
    public class AnswerCheckerService
    {
        public bool CheckSingleChoiceAnswer(SingleChoiceQuestion question, string answer)
        {
            return answer == question.CorrectOption.Name;
        }

        public bool CheckMultipleChoiceAnswer(MultipleChoiceQuestion question, ICollection<Option> answer)
        {
            var correctAnswers = new HashSet<Option>(question.CorrectOptions);
            return correctAnswers.SetEquals(answer);
        }

        public bool CheckOpenTextAnswer(OpenTextQuestion question, string answer)
        {
            return answer == question.CorrectAnswer;
        }
    }
}
