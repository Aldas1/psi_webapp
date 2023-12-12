import axios from "axios";
import { ExplanationDto, QuestionResponseDto } from "../types/quiz";

async function getQuestions(quizId: number) {
  const response = await axios.get<QuestionResponseDto[]>(
    `/api/quizzes/${quizId}/questions`
  );
  return response.data;
}

async function getExplanation(
  quizId: number,
  questionId: number
): Promise<ExplanationDto> {
  try {
    const response = await axios.get<ExplanationDto>(
      `/api/quizzes/${quizId}/questions/${questionId}/explanation`
    );
    return response.data;
  } catch {
    return {};
  }
}

export { getQuestions, getExplanation };
