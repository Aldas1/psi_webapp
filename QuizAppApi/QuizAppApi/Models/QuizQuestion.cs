namespace QuizAppApi.Models
{
    public abstract class QuizQuestion
    {
        public string QuestionText { get; set; }
        public int CorrectOptionIndex { get; set; }
        public string QuestionType { get; set; }
        public QuizAnswer CorrectAnswer { get; set; }
        public abstract string GetQuestionType();

        protected QuizQuestion(string questionText, QuizAnswer correctAnswer)
        {
            QuestionText = questionText;
            QuestionType = GetQuestionType();
            CorrectAnswer = correctAnswer;
        }
        public abstract Object GenerateApiParameters();
        
        public bool IsAnswerCorrect(QuizAnswer answer)
        {
            // Check if the provided answer is correct
            return CorrectAnswer.Equals(answer);
        }
    }
}
