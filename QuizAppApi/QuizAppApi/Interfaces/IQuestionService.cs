using QuizAppApi.DTOs;

namespace QuizAppApi.Interfaces;
public interface IQuestionService
{
    IEnumerable<QuestionResponseDTO>? GetQuestions(int id);
}
