using Microsoft.AspNetCore.Mvc;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers
{
    [ApiController]
    [Route("quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost]
        public ActionResult<QuizCreationResponseDTO> CreateQuiz([FromBody] QuizCreationRequestDTO request)
        {
            return _quizService.CreateQuiz(request);
        }
            
        [HttpGet]
        public ActionResult<IEnumerable<QuizResponseDTO>> GetQuizzes()
        {
            var quizzes = _quizService.GetQuizzes();
            if (quizzes == null)
            {
                return NotFound();
            }
            return Ok(quizzes);
        }

        // TODO: Add details
        [HttpGet("{id}/questions")]
        public ActionResult<IEnumerable<QuestionResponseDTO>> GetQuestions(int id)
        {
            var questions = _quizService.GetQuestions(id);
            if (questions == null)
            {
                return NotFound();
            }
            return Ok(questions);
        }

        [HttpPost("{id}/submit")]
        public ActionResult<AnswerSubmitResponseDTO> SubmitAnswers(int id, [FromBody] List<AnswerSubmitRequestDTO> request)
        {
            return _quizService.SubmitAnswers(id, request);
        }
    }
}
