namespace QuizAppApi.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int NumberOfSubmitters { get; set; }
        
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
