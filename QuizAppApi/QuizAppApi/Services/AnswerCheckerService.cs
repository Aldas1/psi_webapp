using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using System.Collections.Generic;
using QuizAppApi.Utils;


namespace QuizAppApi.Services
{
    public class AnswerCheckerService : IAnswerCheckerService
    {
        public bool CheckSingleChoiceAnswer(SingleChoiceQuestion question, string answer)
        {
            return answer == question.CorrectOptionName;
        }

        public bool CheckMultipleChoiceAnswer(MultipleChoiceQuestion question, ICollection<Option> answer)
        {
            //var correctAnswers = new HashSet<MultipleChoiceOption>(question.MultipleChoiceOptions);
            var correctAnswers = new HashSet<Option>(question.Options.Where(opt => opt.Correct), new OptionEqualityComparer());


            return correctAnswers.SetEquals(answer);
        }

        public bool CheckOpenTextAnswer(OpenTextQuestion question, string answer)
        {
            return answer == question.CorrectAnswer;
        }
    }
}