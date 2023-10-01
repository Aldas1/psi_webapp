using QuizAppApi.DTOs;

namespace QuizAppApi.Interfaces
{
    public interface IQuizService
    {
        QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request);
        IEnumerable<QuizResponseDTO> GetQuizzes();
        IEnumerable<QuestionResponseDTO>? GetQuestions(int id);
        AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request);
    }
}