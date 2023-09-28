using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Services
{
    public class QuizService : IQuizService
    {
        public QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionResponseDTO> GetQuestions(int id)
        {
            throw new NotImplementedException();
            //var quiz = _quizzes.Find(q => q.Id == id);
            //if (quiz == null)
            //    return NotFound();
            //List<QuestionResponseDTO> questions = new();
            //foreach (var question in quiz.Questions)
            //{
            //    //questions.Add(new QuestionResponseDTO { Text = question.Text, QuestionType = question.QuestionType, QuestionParameters = question.GenerateApiParameters()  });
            //}
            //return questions;
        }

        public IEnumerable<QuizResponseDTO> GetQuizzes()
        {
            throw new NotImplementedException();
            //// TODO: (Maybe) Posibly use LINQ here
            //List<QuizResponseDTO> quizesResponse = new();
            //foreach (var quiz in _quizzes)
            //{
            //    quizesResponse.Add(new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id });
            //}
            //return quizesResponse;
        }

        public AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request)
        {
            throw new NotImplementedException();
        }
    }
}
