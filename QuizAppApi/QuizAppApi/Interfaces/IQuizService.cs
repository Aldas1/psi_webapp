﻿using QuizAppApi.DTOs;

namespace QuizAppApi.Interfaces;
public interface IQuizService
{
    QuizManipulationResponseDTO CreateQuiz(QuizManipulationRequestDTO request);
    QuizManipulationResponseDTO UpdateQuiz(int id, QuizManipulationRequestDTO editRequest);
    IEnumerable<QuestionResponseDTO>? GetQuestions(int id);
    IEnumerable<QuizResponseDTO> GetQuizzes();
    QuizResponseDTO? GetQuiz(int id);
    bool DeleteQuiz(int id);
}