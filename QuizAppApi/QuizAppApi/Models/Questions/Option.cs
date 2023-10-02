namespace QuizAppApi.Models.Questions
{
    public class Option : IEquatable<Option>
    {
        // public int Id { get; set; }
        public string Name { get; set; }

        public bool Equals(Option? other)
        {
            return Name == other?.Name;
        }
    }
}
