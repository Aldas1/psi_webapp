import axios from "axios";
import {
  AnswerSubmitRequestDto,
  AnswerSubmitResponseDto,
  QuestionResponseDto,
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

async function getQuestions(id: number) {
  const response = await axios.get<QuestionResponseDto[]>(
    `/api/quizzes/${id}/questions`
  );
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
  getQuestions,
  createQuiz,
  updateQuiz,
  deleteQuiz,
  submitAnswers,
};
