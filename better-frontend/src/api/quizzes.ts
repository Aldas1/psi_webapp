import axios from "axios";
import {
  AnswerSubmitRequestDto,
  AnswerSubmitResponseDto,
  QuizManipulationRequestDto,
  QuizManipulationResponseDto,
  QuizResponseDto,
} from "../types";

let token: string | undefined;

function setToken(newToken: string | undefined) {
  token = newToken;
}

function genConfig() {
  return token === null
    ? {}
    : {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      };
}

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
    quiz,
    genConfig()
  );
  return response.data;
}

async function updateQuiz(id: number, quiz: QuizManipulationRequestDto) {
  const response = await axios.put<QuizManipulationResponseDto>(
    `/api/quizzes/${id}`,
    quiz,
    genConfig()
  );
  if (response.data.status !== "success") {
    throw new Error(response.data.status);
  }
  return response.data;
}

async function deleteQuiz(id: number) {
  return axios.delete(`/api/quizzes/${id}`, genConfig());
}

async function submitAnswers(
  id: number,
  answers: AnswerSubmitRequestDto[],
  token: string | null
) {
  const config =
    token === null
      ? {}
      : {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        };
  const response = await axios.post<AnswerSubmitResponseDto>(
    `/api/answers/${id}/submit`,
    answers,
    config
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
  setToken,
};
