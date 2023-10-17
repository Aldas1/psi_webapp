using QuizAppApi.DTOs;

namespace QuizAppApi.Interfaces
{
    public interface IQuizService
    {
        QuizManipulationResponseDTO CreateQuiz(QuizManipulationRequestDTO request);
        QuizManipulationResponseDTO UpdateQuiz(int id, QuizManipulationRequestDTO editRequest);
        IEnumerable<QuestionResponseDTO>? GetQuestions(int id);
        IEnumerable<QuizResponseDTO> GetQuizzes();
        QuizResponseDTO? GetQuiz(int id);
        AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request);
        bool DeleteQuiz(int id);
    }
}