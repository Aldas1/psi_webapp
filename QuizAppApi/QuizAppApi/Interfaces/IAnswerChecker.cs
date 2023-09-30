namespace QuizAppApi.Interfaces
{
    public interface IAnswerChecker<TQuestion,TAnswer>
    {
        bool IsCorrect(TQuestion question, TAnswer answer);
    }
}