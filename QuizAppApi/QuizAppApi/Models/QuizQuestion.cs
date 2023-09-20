namespace QuizAppApi.Models
{
    public abstract class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public QuizAnswer CorrectAnswer { get; set; }
        public abstract string GetQuestionType();

        protected QuizQuestion(string questionText, QuizAnswer correctAnswer)
        {
            QuestionText = questionText;
            QuestionType = GetQuestionType();
            CorrectAnswer = correctAnswer;
        }
    }
}
