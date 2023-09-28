using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using System.Text.Json;

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
            var newQuiz = new Quiz(request.Name);
            foreach(var question in  request.Questions) 
            {
                //deserializining json string into a dictionary
                Dictionary<string, JsonElement> data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(question.QuestionParameters);
                Dictionary<string, JsonElement> parameters = (Dictionary<string, JsonElement>)data["parameters"];

                switch (question.QuestionType.ToLowerInvariant())
                {
                    case "singlechoicequestion":
                        {
                            newQuiz.Questions.Add(SingleChoiceQuizQuestion.CreateFromParameters(question.QuestionText, parameters));
                            break;
                        }
                    default:
                        return BadRequest(new QuizCreationResponse { Status = "failed" });
                }


            }
            _quizzes.Add(newQuiz);
            return CreatedAtAction(nameof(GetQuestions), new { id = newQuiz.Id }, new QuizCreationResponse { Status = "success" });
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
        public List<QuestionRequest> Questions { get; set; }
    }

    public class QuestionRequest
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public Dictionary<string, JsonElement> QuestionParameters { get; set; }
    }

    public class QuestionResponse
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public Object QuestionParameters { get; set; }//pakeiciu
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
