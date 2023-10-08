import axios from "axios";
import {
  AnswerSubmitRequestDto,
  AnswerSubmitResponseDto,
  QuestionResponseDto,
  QuizCreationRequestDto,
  QuizCreationResponseDto,
  QuizResponseDto,
} from "../types";

async function getQuizzes() {
  const response = await axios.get<QuizResponseDto[]>("/api/quizzes");
  return response.data;
}

async function getQuestions(id: number) {
  const response = await axios.get<QuestionResponseDto[]>(
    `/api/quizzes/${id}/questions`
  );
  return response.data;
}

async function createQuiz(quiz: QuizCreationRequestDto) {
  const response = await axios.post<QuizCreationResponseDto>(
    "/api/quizzes",
    quiz
  );
  return response.data;
}

async function submitAnswers(id: number, answers: AnswerSubmitRequestDto[]) {
  const response = await axios.post<AnswerSubmitResponseDto>(
    `/api/quizzes/${id}/submit`,
    answers
  );
  return response.data;
}

export { getQuizzes, getQuestions, createQuiz, submitAnswers };
