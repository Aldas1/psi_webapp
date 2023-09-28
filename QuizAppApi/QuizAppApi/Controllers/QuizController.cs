using Microsoft.AspNetCore.Mvc;
using QuizAppApi.DTOs;

namespace QuizAppApi.Controllers
{
    [ApiController]
    [Route("quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly QuizStorage _quizzes = QuizStorage.Instance;

        [HttpPost]
        public ActionResult<QuizCreationResponseDTO> CreateQuiz([FromBody] QuizCreationQuestionRequestDTO request)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizResponseDTO>> ListQuizzes()
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

        // TODO: Add details
        [HttpGet("{id}/questions")]
        public ActionResult<IEnumerable<QuestionResponseDTO>> GetQuestions(int id)
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

        [HttpPost("{id}/submit")]
        public ActionResult<AnswerSubmitResponseDTO> SubmitAnswers(int id, [FromBody] List<AnswerSubmitRequestDTO> request)
        {
            throw new NotImplementedException();
        }
    }
}
