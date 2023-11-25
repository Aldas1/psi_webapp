namespace QuizAppApi;

public static class Config
{
    public struct QuestionTypeValues
    {
        public const string SingleChoiceQuestionApiString = "singleChoiceQuestion";
        public const string MultipleChoiceQuestionApiString = "multipleChoiceQuestion";
        public const string OpenTextQuestionApiString = "openTextQuestion";
    }
    public struct ReadableQuestionTypeValues
    {
        public const string SingleChoiceQuestionApiString = "single choice question";
        public const string MultipleChoiceQuestionApiString = "multiple choice question";
        public const string OpenTextQuestionApiString = "open text question";
    }
}