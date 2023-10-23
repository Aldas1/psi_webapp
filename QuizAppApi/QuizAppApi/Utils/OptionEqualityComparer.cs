using QuizAppApi.Models.Questions;
using System.Diagnostics.CodeAnalysis;

namespace QuizAppApi.Utils;

public class OptionEqualityComparer : IEqualityComparer<Option>
{
    public bool Equals(Option? x, Option? y)
    {
        if (x is null || y is null) return false;
        return x.Name == y.Name;
    }

    public int GetHashCode([DisallowNull] Option obj)
    {
        return obj.Name.GetHashCode();
    }
}