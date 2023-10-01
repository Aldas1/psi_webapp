using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

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
            var response = new AnswerSubmitResponseDTO();
            var quiz = _quizRepository.GetQuizById(id);
            var correctAnswers = 0;

            foreach (var answer in request)
            {
                var question = quiz.Questions.FirstOrDefault(q => q.Id == answer.QuestionId) as SingleChoiceQuestion;
                var selectedOption = question?.Options.FirstOrDefault(o => o.Name == answer.OptionName);
                if (question != null && selectedOption != null)
                {
                    var checker = new SingleChoiceAnswerChecker();
                    var isCorrect = checker.IsCorrect(question, selectedOption);

                    if (isCorrect)
                    {
                        correctAnswers++;
                    }
                }
            }

            response.CorrectlyAnswered = correctAnswers;

            return response;
        }
    }
}
