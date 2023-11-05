import axios from "axios";
import { QuestionResponseDto } from "../types";

async function getQuestions(quiz_id: number) {
  const response = await axios.get<QuestionResponseDto[]>(
    `/api/questions/${quiz_id}/quiz_questions`
  );
  return response.data;
}

export { getQuestions };
