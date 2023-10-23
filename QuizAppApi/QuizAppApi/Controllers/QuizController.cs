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

        [HttpGet("{id}")]
        public ActionResult<QuizResponseDTO> GetQuiz(int id)
        {
            var quiz = _quizService.GetQuiz(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return Ok(quiz);
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
        public async Task<ActionResult<AnswerSubmitResponseDTO>> SubmitAnswers(int id, [FromBody] List<AnswerSubmitRequestDTO> request)
        {
            return await _quizService.SubmitAnswers(id, request);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteQuiz(int id)
        {
            bool response = _quizService.DeleteQuiz(id);
            if (response)
            {
                return Ok();

            } else
            {
                return BadRequest();

            }
        }
    }
}
