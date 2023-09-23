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
        
        public bool SubmitAnswers(List<SingleChoiceQuizAnswer> answers)
        {
            foreach (var question in Questions)
            {
                var matchingAnswer = answers.FirstOrDefault(answer => answer.Question == question);
                if (matchingAnswer != null)
                {
                    bool isCorrect = matchingAnswer.OptionIndex == question.CorrectOptionIndex;
                    // Update the question state or record the correctness as needed.
                }
            }

            // You can also update the NumberOfSubmitters here.
            return true; // Indicate success
        }
    }
}
