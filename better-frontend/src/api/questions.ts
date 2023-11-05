import axios from "axios";
import { QuestionResponseDto } from "../types";

async function getQuestions(quiz_id: number) {
  const response = await axios.get<QuestionResponseDto[]>(
    `/api/quizzes/${quiz_id}/questions/quiz_questions`
  );
  return response.data;
}

export { getQuestions };
