import axios from "axios";
import { QuestionResponseDto } from "../types/quiz";

async function getQuestions(quiz_id: number) {
  const response = await axios.get<QuestionResponseDto[]>(
    `/api/quizzes/${quiz_id}/questions`
  );
  return response.data;
}

export { getQuestions };
