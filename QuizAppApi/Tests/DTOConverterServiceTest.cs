using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;
using System.Linq;

namespace Tests
{
	[TestFixture]
	public class QuestionDTOConverterServiceTests
	{
		private SingleChoiceQuestionDTOConverterService _singleChoiceConverter;
		private MultipleChoiceQuestionDTOConverterService _multipleChoiceConverter;
		private OpenTextQuestionDTOConverterService _openTextConverter;

		[SetUp]
		public void Setup()
		{
			_singleChoiceConverter = new SingleChoiceQuestionDTOConverterService();
			_multipleChoiceConverter = new MultipleChoiceQuestionDTOConverterService();
			_openTextConverter = new OpenTextQuestionDTOConverterService();
		}

		[Test]
		public void SingleChoiceCreateFromParameters_ReturnsSingleChoiceQuestion()
		{
			// Arrange
			var questionDTO = new QuestionParametersDTO
			{
				Options = new List<string> { "Option1", "Option2", "Option3" },
				CorrectOptionIndex = 0
			};

			// Act
			var result = _singleChoiceConverter.CreateFromParameters(questionDTO);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<SingleChoiceQuestion>(result);
			Assert.AreEqual(3, result.Options.Count);
			Assert.IsTrue(result.Options.All(option => option.Name == "Option1" || option.Name == "Option2" || option.Name == "Option3"));
			Assert.IsTrue(result.Options.Any(option => option.Correct));
		}

		[Test]
		public void SingleChoiceCreateFromParameters_ThrowsDTOConversionException()
		{
			// Arrange
			var questionDTO = new QuestionParametersDTO
			{
				Options = new List<string> { "Option1", "Option2", "Option3" },
				CorrectOptionIndex = 3
			};

			// Act & Assert
			Assert.Throws<DTOConversionException>(() => _singleChoiceConverter.CreateFromParameters(questionDTO));
		}

		[Test]
		public void SingleChoiceGenerateParameters_ReturnsQuestionParametersDTO()
		{
			// Arrange
			var question = new SingleChoiceQuestion
			{
				Options = new List<Option>
				{
					new Option { Name = "Option1", Correct = true },
					new Option { Name = "Option2", Correct = false },
				}
			};

			// Act
			var result = _singleChoiceConverter.GenerateParameters(question);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<QuestionParametersDTO>(result);
			Assert.AreEqual(2, result.Options.Count);
			Assert.IsTrue(result.Options.All(option => option == "Option1" || option == "Option2"));
			Assert.AreEqual(0, result.CorrectOptionIndex);
		}

		[Test]
		public void MultipleChoiceCreateFromParameters_ReturnsMultipleChoiceQuestion()
		{
			// Arrange
			var questionDTO = new QuestionParametersDTO
			{
				Options = new List<string> { "Option1", "Option2", "Option3" },
				CorrectOptionIndexes = new List<int> { 0, 2 }
			};

			// Act
			var result = _multipleChoiceConverter.CreateFromParameters(questionDTO);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MultipleChoiceQuestion>(result);
			Assert.AreEqual(3, result.Options.Count);
			Assert.IsTrue(result.Options.All(option => option.Name == "Option1" || option.Name == "Option2" || option.Name == "Option3"));
			Assert.IsTrue(result.Options.Where(option => option.Correct).Count() == 2);
		}

		[Test]
		public void MultipleChoiceCreateFromParameters_ThrowsDTOConversionException()
		{
			// Arrange
			var questionDTO = new QuestionParametersDTO
			{
				Options = new List<string> { "Option1", "Option2", "Option3" },
				CorrectOptionIndexes = new List<int> { 0, 3 } // Index 3 is out of bounds
			};

			// Act & Assert
			Assert.Throws<DTOConversionException>(() => _multipleChoiceConverter.CreateFromParameters(questionDTO));
		}

		[Test]
		public void MultipleChoiceGenerateParameters_ReturnsQuestionParametersDTO()
		{
			// Arrange
			var question = new MultipleChoiceQuestion
			{
				Options = new List<Option>
				{
					new Option { Name = "Option1", Correct = true },
					new Option { Name = "Option2", Correct = false },
					new Option { Name = "Option3", Correct = true },
				}
			};

			// Act
			var result = _multipleChoiceConverter.GenerateParameters(question);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<QuestionParametersDTO>(result);
			Assert.AreEqual(3, result.Options.Count);
			Assert.IsTrue(result.Options.All(option => option == "Option1" || option == "Option2" || option == "Option3"));
			Assert.IsTrue(result.CorrectOptionIndexes.Count() == 2);
		}

		[Test]
		public void OpenTextCreateFromParameters_ReturnsOpenTextQuestion()
		{
			// Arrange
			var questionDTO = new QuestionParametersDTO
			{
				CorrectText = "ValidAnswer?"
			};

			// Act
			var result = _openTextConverter.CreateFromParameters(questionDTO);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<OpenTextQuestion>(result);
			Assert.AreEqual("ValidAnswer?", result.CorrectAnswer);
		}

		[Test]
		public void OpenTextCreateFromParameters_ThrowsDTOConversionException()
		{
			// Arrange
			var questionDTO = new QuestionParametersDTO
			{
				CorrectText = "#InvalidAnswer"
			};

			// Act & Assert
			Assert.Throws<DTOConversionException>(() => _openTextConverter.CreateFromParameters(questionDTO));
		}

		[Test]
		public void OpenTextGenerateParameters_ReturnsQuestionParametersDTO()
		{
			// Arrange
			var question = new OpenTextQuestion
			{
				CorrectAnswer = "ValidAnswer?"
			};

			// Act
			var result = _openTextConverter.GenerateParameters(question);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<QuestionParametersDTO>(result);
			Assert.AreEqual("ValidAnswer?", result.CorrectText);
		}
	}
}