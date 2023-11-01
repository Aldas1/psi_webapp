import axios from "axios";
import { QuestionResponseDto } from "../types";

async function getQuestions(id: number) {
  const response = await axios.get<QuestionResponseDto[]>(
    `/api/questions/${id}/questions`
  );
  return response.data;
}

export { getQuestions };
