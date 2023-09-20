namespace QuizAppApi.Models
{
    public abstract class QuizAnswer : IEquatable<QuizAnswer>
    {
        // Compares two answers and returns true if answers are the same (or using some other logic)
        public abstract bool IsCorrect(QuizAnswer answer);

        public bool Equals(QuizAnswer? other)
        {
           if (other == null) return false;
            return this.IsCorrect(other);
        }

        protected QuizAnswer() { }
    }

}
