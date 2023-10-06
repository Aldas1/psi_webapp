namespace QuizAppApi
{
    public static class Config
    {
        public struct ConfigValues
        {
            public string SingleChoiceQuestionApiString { get; set; }

            public string MultipleChoiceQuestionApiString { get; set; }

            public string OpenTextQuestionApiString { get; set; }
        }

        public static ConfigValues QuestionTypeValues { get; } = new ConfigValues
        {
            SingleChoiceQuestionApiString = "singleChoiceQuestion",
            MultipleChoiceQuestionApiString = "multipleChoiceQuestion",
            OpenTextQuestionApiString = "openTextQuestion"
        };
    }
}
