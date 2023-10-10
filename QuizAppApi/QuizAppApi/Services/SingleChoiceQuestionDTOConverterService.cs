﻿using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Services
{
    public class SingleChoiceQuestionDTOConverterService : IQuestionDTOConverterService<SingleChoiceQuestion>
    {
        public SingleChoiceQuestion? CreateFromParameters(QuestionParametersDTO questionDTO)
        {
            var options = questionDTO.Options;
            var correctOptionIndex = questionDTO.CorrectOptionIndex;

            if (options == null || correctOptionIndex == null || correctOptionIndex < 0 || correctOptionIndex >= options.Count)
            {
                return null;
            }

            return new SingleChoiceQuestion
            {
                Options = options.Select(o => new Option { Name = o }).ToList(),
                CorrectOption = new Option { Name = options[(int)correctOptionIndex] }
            };
        }

        public QuestionParametersDTO GenerateParameters(SingleChoiceQuestion question)
        {
            return new QuestionParametersDTO { Options = question.Options.Select(opt => opt.Name).ToList() };
        }
    }
}