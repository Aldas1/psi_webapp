using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Utils;

namespace QuizAppApi.Services
{
    public class QuizService : IQuizService
    {
        private IQuizRepository _quizRepository;
        private SingleChoiceAnswerChecker _singleChoiceAnswerChecker;

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
            // throw new NotImplementedException();
            var response = new AnswerSubmitResponseDTO();
            var correctAnswers = 0;


            foreach (var answer in request)
            {
                var isCorrect = _singleChoiceAnswerChecker.IsCorrect(answer.QuestionId, answer.OptionIndex);

                if (isCorrect)
                {
                    correctAnswers++;
                }
            }

            response.CorrectlyAnswered = correctAnswers;

            return response;
        }
    }
}
