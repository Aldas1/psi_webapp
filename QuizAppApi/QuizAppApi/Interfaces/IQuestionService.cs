using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;
public interface IQuestionService
{
    IEnumerable<QuestionResponseDto>? GetQuestions(int id);
}
