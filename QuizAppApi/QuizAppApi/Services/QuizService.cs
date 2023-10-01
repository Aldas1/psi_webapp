using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionResponseDTO>? GetQuestions(int id)
        {
            var quiz = _quizRepository.GetQuizById(id);
            if (quiz == null)
            {
                return null;
            }
            var questions = new List<QuestionResponseDTO>();
            foreach (var question in quiz.Questions)
            {
                var questionResponse = new QuestionResponseDTO
                    { Id = question.Id, QuestionText = question.Text, QuestionType = question.Type};
                switch (question)
                {
                    case SingleChoiceQuestion singleChoiceQuestion:
                        questionResponse.QuestionParameters = new QuestionParametersDTO
                        {
                            Options = singleChoiceQuestion.Options.Select(opt => opt.Name).ToList(),
                        };
                        questions.Add(questionResponse);
                        break;
                }
            }
            return questions;
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