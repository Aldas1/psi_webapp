using NUnit.Framework;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;
using System.Collections.Generic;

namespace Tests;

[TestFixture]
public class OptionEqualityComparerTests
{
    [Test]
    public void Equals_OptionsWithSameName_ReturnsTrue()
    {
        var option1 = new Option { Name = "OptionName" };
        var option2 = new Option { Name = "OptionName" };
        var optionEqualityComparer = new OptionEqualityComparer();

        var result = optionEqualityComparer.Equals(option1, option2);

        Assert.IsTrue(result);
    }

    [Test]
    public void Equals_OptionsWithDifferentNames_ReturnsFalse()
    {
        var option1 = new Option { Name = "OptionName1" };
        var option2 = new Option { Name = "OptionName2" };
        var optionEqualityComparer = new OptionEqualityComparer();

        var result = optionEqualityComparer.Equals(option1, option2);

        Assert.IsFalse(result);
    }

    [Test]
    public void GetHashCode_ReturnsHashCodeOfOptionName()
    {
        var option = new Option { Name = "OptionName" };
        var optionEqualityComparer = new OptionEqualityComparer();

        var hashCode = optionEqualityComparer.GetHashCode(option);

        Assert.AreEqual("OptionName".GetHashCode(), hashCode);
    }
}