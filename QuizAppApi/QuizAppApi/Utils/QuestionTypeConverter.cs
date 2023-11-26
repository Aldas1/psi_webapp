using QuizAppApi.Enums;

namespace QuizAppApi.Utils;

public static class QuestionTypeConverter
{
    public static string ToString(QuestionType type)
    {
        switch (type)
        {
            case QuestionType.SingleChoiceQuestion:
                return Config.QuestionTypeValues.SingleChoiceQuestionApiString;
                    
            case QuestionType.MultipleChoiceQuestion:
                return Config.QuestionTypeValues.MultipleChoiceQuestionApiString;

            case QuestionType.OpenTextQuestion:
                return Config.QuestionTypeValues.OpenTextQuestionApiString;

            default:
                return string.Empty;
        }
    }
    public static string ToReadableString(QuestionType type)
    {
        switch (type)
        {
            case QuestionType.SingleChoiceQuestion:
                return Config.ReadableQuestionTypeValues.SingleChoiceQuestionApiString;

            case QuestionType.MultipleChoiceQuestion:
                return Config.ReadableQuestionTypeValues.MultipleChoiceQuestionApiString;

            case QuestionType.OpenTextQuestion:
                return Config.ReadableQuestionTypeValues.OpenTextQuestionApiString;

            default:
                return string.Empty;
        }
    }

    public static QuestionType FromString(string type) 
    {
        switch(type)
        {
            case Config.QuestionTypeValues.SingleChoiceQuestionApiString:
                return QuestionType.SingleChoiceQuestion;
            case Config.QuestionTypeValues.MultipleChoiceQuestionApiString:
                return QuestionType.MultipleChoiceQuestion;
            case Config.QuestionTypeValues.OpenTextQuestionApiString:
                return QuestionType.OpenTextQuestion;
            default:
                return QuestionType.Unknown;
        }
    }
}