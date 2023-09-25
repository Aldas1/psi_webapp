using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Controllers
{
    [ApiController]
    [Route("quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly QuizStorage _quizzes = QuizStorage.Instance;

        [HttpPost]
        public ActionResult<QuizCreationResponse> CreateQuiz([FromBody] QuizCreationRequest request)
        {
            //sukurti quiz obj ir priskirti name reiksme
            //questionu obj ir i quiz obj
            var new_quiz = new Quiz(request.Name, 0);
            request.Questions.Select(question => new_quiz.Questions.Add(
                //pakeist kad acceptintu bet koki typea
                new SingleChoiceQuizQuestion(request.Questions., , )
            ));
            
            _quizzes.Add(new Quiz(request.Name, 0));
            return BadRequest(new QuizCreationResponse { Status = "failed" });
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizResponse>> ListQuizzes()
        {
            var quizResponses = _quizzes.Select(quiz => new QuizResponse { Name = quiz.Name, Id = quiz.Id }).ToList();
            return quizResponses;
        }

        // TODO: Add details
        [HttpGet("{id}/questions")]
        public ActionResult<IEnumerable<QuestionResponse>> GetQuestions(Guid id)
        {
            var quiz = _quizzes.Find(q => q.Id == id);
            if (quiz == null)
                return NotFound();
            List<QuestionResponse> questions = new();
            foreach (var question in quiz.Questions)
            {
                questions.Add(new QuestionResponse { QuestionText = question.QuestionText, QuestionType = question.QuestionType, QuestionParameters = question.GenerateApiParameters()  });
            }
            return questions;
        }

        [HttpPost("{id}/submit")]
        public ActionResult<SubmitResponse> SubmitAnswers(Guid id, [FromBody] SubmitRequest request)
        {
            return BadRequest(new { StatusCode = "Not implemented" });
        }
    }

    public class QuizCreationResponse
    {
        public string Status { get; set; }
    }

    public class QuizCreationRequest
    {
        public string Name { get; set; }
        public List<QuestionRequest> Questions { get; set; } //listas klausimu
    }

    public class QuestionRequest
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public Object QuestionParameters { get; set; }
    }

    public class QuestionResponse
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public Object QuestionParameters { get; set; }
    }

    public class QuizResponse
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }

    public class SubmitResponse
    {

    }

    public class SubmitRequest
    {

    }
}
