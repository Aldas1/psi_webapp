using QuizAppApi.Controllers;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Models
{
    public class Quiz
    {
        public string Name { get; set; }
        public int NumberOfSubmitters { get; set; }
        public Guid Id { get; }
        public List<QuizQuestion> Questions { get; set; }

        public Quiz(string name, int numberOfSubmitters = 0)
        {
            Name = name;
            NumberOfSubmitters = numberOfSubmitters;
            Id = Guid.NewGuid();
            Questions = new List<QuizQuestion>();
        }

        public bool SubmitAnswers(List<QuizController.QuizAnswerRequest> answers)
        {
            foreach (var submission in answers)
            {
                if (submission.QuestionIndex >= 0 && submission.QuestionIndex < Questions.Count)
                {
                    var question = Questions[submission.QuestionIndex];

                    if (submission.CorrectOptionIndex >= 0)
                    {
                        // Handle single-choice questions
                        if (question is SingleChoiceQuizQuestion singleChoiceQuestion)
                        {
                            var selectedAnswer = new SingleChoiceQuizAnswer(submission.CorrectOptionIndex);
                            bool isCorrect = selectedAnswer.IsCorrect(singleChoiceQuestion.CorrectAnswer);
                            // Update the question state or record the correctness as needed.
                        }
                        // Add handling for other question types (if applicable)
                    }
                    else if (!string.IsNullOrEmpty(submission.AnswerString))
                    {
                        // Handle other question types that require a text answer (e.g., short answer)
                        // You would need to implement the logic to check the correctness based on the provided answer.
                    }
                }
            }
            
            NumberOfSubmitters++;

            return true;
        }

    }
}
