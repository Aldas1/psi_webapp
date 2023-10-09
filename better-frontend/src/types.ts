interface QuizResponseDto {
  id: number;
  name: string;
}

interface QuizCreationQuestionRequestDto {
  questionText: string;
  questionType: QuestionTypeDto;
  questionParameters: QuestionParametersDto;
  id?: number;
}

interface QuizCreationRequestDto {
  name: string;
  questions: QuizCreationQuestionRequestDto[];
  id?: number;
}

interface QuizCreationResponseDto {
  status: string;
  id: number;
}

type QuestionTypeDto =
  | "singleChoiceQuestion"
  | "multipleChoiceQuestion"
  | "openTextQuestion";

interface QuestionParametersDto {
  options?: string[];
  correctOptionIndex?: number;
}

interface QuestionResponseDto {
  id: number;
  questionText: string;
  questionType: QuestionTypeDto;
  questionParameters: QuestionParametersDto;
}

interface AnswerSubmitRequestDto {
  questionId: number;
  optionName: string;
}

interface AnswerSubmitResponseDto {
  score: number;
  correctlyAnswered: number;
  status: string;
}

export type {
  QuestionTypeDto,
  QuestionParametersDto,
  QuizCreationRequestDto,
  QuizCreationQuestionRequestDto,
  QuizCreationResponseDto,
  QuizResponseDto,
  QuestionResponseDto,
  AnswerSubmitRequestDto,
  AnswerSubmitResponseDto,
};
