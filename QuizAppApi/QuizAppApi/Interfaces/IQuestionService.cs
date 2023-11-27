using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;
public interface IQuestionService
{
    Task<IEnumerable<QuestionResponseDto>?> GetQuestionsAsync(int id);
    Task<ExplanationDto?> GetQuestionWithExplanationAsync(int quizId, int questionId);
}
