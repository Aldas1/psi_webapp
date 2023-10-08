using QuizAppApi.DTOs;

namespace QuizAppApi.Interfaces
{
    public interface IQuizService
    {
        QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request);
        IEnumerable<QuestionResponseDTO>? GetQuestions(int id);
        IEnumerable<QuizResponseDTO> GetQuizzes();
        AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request);
        bool DeleteQuiz(int id);
    }
}