namespace QuizAppApi.Models
{
    public abstract class QuizAnswer
    {
        public static bool operator==(QuizAnswer a, QuizAnswer b)
        {
            return a.IsCorrect(b);
        }

        public static bool operator!=(QuizAnswer a, QuizAnswer b)
        {
            return !a.IsCorrect(b);
        }

        // Compares two answers and returns true if answers are the same (or using some other logic)
        public abstract bool IsCorrect(QuizAnswer answer);
        protected QuizAnswer() { }
    }
}
