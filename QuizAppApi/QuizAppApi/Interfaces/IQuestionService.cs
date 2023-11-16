﻿using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;
public interface IQuestionService
{
    Task<IEnumerable<QuestionResponseDto>?> GetQuestionsAsync(int id);
}
