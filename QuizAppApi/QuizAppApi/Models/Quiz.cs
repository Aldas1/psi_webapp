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

    }
}
