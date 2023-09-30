namespace QuizAppApi.DTOs
{
    public class AnswerSubmitResponseDTO
    {
        public int Score { get; set; }
        public int CorrectlyAnswered { get; set; } // number of correctly answered questions
    }
}
