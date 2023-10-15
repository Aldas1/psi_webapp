using QuizAppApi.Models.Questions;
using System.Diagnostics.CodeAnalysis;

namespace QuizAppApi.Utils
{
    public class MultipleChoiceOptionEqualityComparer : IEqualityComparer<MultipleChoiceOption>
    {
        public bool Equals(MultipleChoiceOption? x, MultipleChoiceOption? y)
        {
            if (x is null || y is null) return false;
            return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] MultipleChoiceOption obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
