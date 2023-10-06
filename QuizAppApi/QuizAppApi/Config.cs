namespace QuizAppApi
{
    public static class Config
    {
        public struct QuestionTypeValues
        {
            public const string SingleChoiceQuestionApiString = "singleChoiceQuestion";
            public const string MultipleChoiceQuestionApiString = "multipleChoiceQuestion";
            public const string OpenTextQuestionApiString = "openTextQuestion";
        }

        //public static QuestionType QuestionTypeValues { get; } = new QuestionType();
    }
}
