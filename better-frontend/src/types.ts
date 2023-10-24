interface QuizResponseDto {
  id: number;
  name: string;
}

interface QuizManipulationQuestionRequestDto {
  questionText: string;
  questionType: QuestionTypeDto;
  questionParameters: QuestionParametersDto;
  id?: number;
}

interface QuizManipulationRequestDto {
  name: string;
  questions: QuizManipulationQuestionRequestDto[];
  id?: number;
}

interface QuizManipulationResponseDto {
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
  correctOptionIndexes?: number[];
  correctText?: string;
}

interface QuestionResponseDto {
  id: number;
  questionText: string;
  questionType: QuestionTypeDto;
  questionParameters: QuestionParametersDto;
}

interface AnswerSubmitRequestDto {
  questionId: number;
  optionName?: string;
  optionNames?: string[];
  answerText?: string;
}

interface AnswerSubmitResponseDto {
  score: number;
  correctlyAnswered: number;
  status: string;
}

export type {
  QuestionTypeDto,
  QuestionParametersDto,
  QuizManipulationRequestDto,
  QuizManipulationQuestionRequestDto,
  QuizManipulationResponseDto,
  QuizResponseDto,
  QuestionResponseDto,
  AnswerSubmitRequestDto,
  AnswerSubmitResponseDto,
};
