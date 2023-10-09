namespace QuizAppApi.Models.Questions
{
    public class Option : IEquatable<Option>
    {
        // public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public bool Equals(Option? other)
        {
            return Name == other?.Name;
        }
    }
}
