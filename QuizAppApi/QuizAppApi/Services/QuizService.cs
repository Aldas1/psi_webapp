using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Services
{
    public class QuizService : IQuizService
    {
        private IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionResponseDTO> GetQuestions(int id)
        {
            throw new NotImplementedException();
            // var quiz = _quizRepository.GetQuizById(id);
            // if (quiz == null)
            // {
            //     return null;
            // }
            // // TODO: Return type
            // return quiz.Questions.Select(quiz => new QuestionResponseDTO { Text = quiz.Text });
        }

        public IEnumerable<QuizResponseDTO> GetQuizzes()
        {
            _quizRepository.GetQuizzes();
            return _quizRepository.GetQuizzes()
                .Select(
                    quiz => new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id }
                );
        }

        public AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request)
        {
            throw new NotImplementedException();
        }
    }
}
