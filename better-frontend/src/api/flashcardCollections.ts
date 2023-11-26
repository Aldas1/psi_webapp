import axios from "axios";
import { FlashcardCollectionDto, FlashcardDto } from "../types/flashcard";

const BASE_URL = "/api/flashcard-collections/";

export async function createFlashcardCollection(
  collection: FlashcardCollectionDto
) {
  const response = await axios.post<FlashcardCollectionDto>(
    BASE_URL,
    collection
  );
  return response.data;
}

export async function getFlashcardCollections() {
  const response = await axios.get<FlashcardCollectionDto[]>(BASE_URL);
  return response.data;
}

export async function getFlashcardCollection(id: number) {
  const response = await axios.get<FlashcardCollectionDto>(`${BASE_URL}${id}`);
  return response.data;
}

export async function getFlashcards(id: number) {
  const response = await axios.get<FlashcardDto[]>(
    `${BASE_URL}${id}/flashcards`
  );
  return response.data;
}
