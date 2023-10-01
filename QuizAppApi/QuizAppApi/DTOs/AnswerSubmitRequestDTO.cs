using QuizAppApi.Models.Questions;

namespace QuizAppApi.DTOs
{
    public class AnswerSubmitRequestDTO
    {
        public SingleChoiceQuestion QuestionId { get; set; }
        public Option? OptionIndex { get; set; }
        // TODO: Add props for other question types
    }
}
