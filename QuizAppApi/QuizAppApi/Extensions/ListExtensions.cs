using QuizAppApi.Models;
using QuizAppApi.Utils;

namespace QuizAppApi.Extensions
{
    public static class ListExtensions
    {
        public static object Clone(this List<Quiz> list)
        {
            var newList = new List<Quiz>();
            foreach (var item in list)
            {
                newList.Add(QuizSerialization.CloneQuiz(item));
            }

            return newList;
        }
    }
}