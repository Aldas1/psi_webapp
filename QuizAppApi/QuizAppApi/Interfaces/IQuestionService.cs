using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;
public interface IQuestionService
{
    Task<IEnumerable<QuestionResponseDto>?> GetQuestions(int id);
}
