import axios from "axios";
import {
  AnswerSubmitRequestDto,
  AnswerSubmitResponseDto,
  QuizManipulationRequestDto,
  QuizManipulationResponseDto,
  QuizResponseDto,
} from "../types";

async function getQuizzes() {
  const response = await axios.get<QuizResponseDto[]>("/api/quizzes");
  return response.data;
}

async function getQuiz(id: number) {
  const response = await axios.get<QuizResponseDto>(`/api/quizzes/${id}`);
  return response.data;
}

async function createQuiz(quiz: QuizManipulationRequestDto) {
  const response = await axios.post<QuizManipulationResponseDto>(
    "/api/quizzes",
    quiz
  );
  return response.data;
}

async function updateQuiz(id: number, quiz: QuizManipulationRequestDto) {
  const response = await axios.put<QuizManipulationResponseDto>(
    `/api/quizzes/${id}`,
    quiz
  );
  if (response.data.status !== "success") {
    throw new Error(response.data.status);
  }
  return response.data;
}

async function deleteQuiz(id: number) {
  return axios.delete(`/api/quizzes/${id}`);
}

async function submitAnswers(id: number, answers: AnswerSubmitRequestDto[]) {
  const response = await axios.post<AnswerSubmitResponseDto>(
    `/api/quizzes/${id}/submit`,
    answers
  );
  return response.data;
}

export {
  getQuizzes,
  getQuiz,
  createQuiz,
  updateQuiz,
  deleteQuiz,
  submitAnswers,
};
