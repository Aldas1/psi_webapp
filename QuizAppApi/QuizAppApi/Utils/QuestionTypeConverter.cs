using QuizAppApi.Enums;

namespace QuizAppApi.Utils
{
    public static class QuestionTypeConverter
    {
        public static string ToString(QuestionType type)
        {
            switch (type)
            {
                case QuestionType.SingleChoiceQuestion:
                    return QuizAppApi.Config.QuestionTypeValues.SingleChoiceQuestionApiString;
                    
                case QuestionType.MultipleChoiceQuestion:
                    return QuizAppApi.Config.QuestionTypeValues.MultipleChoiceQuestionApiString;

                case QuestionType.OpenTextQuestion:
                    return QuizAppApi.Config.QuestionTypeValues.OpenTextQuestionApiString;

                default:
                    return string.Empty;
            }
        }
    }

}