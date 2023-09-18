using Microsoft.AspNetCore.Mvc;

namespace QuizAppApi.Controllers
{
    [ApiController]
    [Route("quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly QuizStorage _quizzes = QuizStorage.Instance;

        [HttpPost]
        public ActionResult<QuizCreationResponse> CreateQuiz([FromBody]QuizCreationRequest request)
        {
            return BadRequest(new QuizCreationResponse { Status = "failed"});
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizReponse>> ListQuizzes()
        {
            // TODO: (Maybe) Posibly use LINQ here
            List<QuizReponse> quizesResponse = new();
            foreach (var quiz in _quizzes)
            {
                quizesResponse.Add(new QuizReponse{ Name = quiz.Name, Id = quiz.Id});
            }
            return quizesResponse;
        }

        // TODO: Add details
        [HttpGet("{id}/questions")]
        public ActionResult<IEnumerable<QuestionResponse>> GetQuestions(Guid id)
        {
            var quiz =_quizzes.Find(q => q.Id == id);
            List<QuestionResponse> questions = new();
            foreach(var question in quiz.Questions)
            {
                questions.Add(new QuestionResponse { QuestionText = question.QuestionText });
            }
            return questions;
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

    }

    public class QuestionResponse
    {
        public string QuestionText { get; set; }
    }

    public class QuizReponse
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
