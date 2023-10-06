using System.ComponentModel;

namespace QuizAppApi.Enums
{
    public enum QuestionType
    {
        [Description("singleChoiceQuestion")]
        SingleChoiceQuestion,

        [Description("multipleChoiceQuestion")]
        MultipleChoiceQuestion,

        [Description("openTextQuestion")]
        OpenTextQuestion
    }
}