namespace QuizAppApi.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSubmitters { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
