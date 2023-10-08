import { useState } from "react";
import {
  AnswerSubmitRequestDto,
  QuestionParametersDto,
  QuizCreationQuestionRequestDto,
  QuizCreationRequestDto,
} from "../types";
import {
  Center,
  Card,
  CardBody,
  Button,
  Flex,
  Box,
  IconButton,
  Container,
  Heading,
  VStack,
  RadioGroup,
  Radio,
  Progress,
} from "@chakra-ui/react";
import { CheckIcon, ChevronLeftIcon, ChevronRightIcon } from "@chakra-ui/icons";

function SingleChoiceControls({
  parameters,
  answer,
  onAnswerChange = () => undefined,
}: {
  parameters: QuestionParametersDto;
  answer?: AnswerSubmitRequestDto;
  onAnswerChange?: (newAnswer: AnswerSubmitRequestDto) => void;
}) {
  const options = parameters.options ?? [];
  const correctName = answer?.optionName || options.join("-");

  return (
    <Box>
      <RadioGroup
        value={correctName}
        onChange={(v) => onAnswerChange({ optionName: v, questionId: 0 })}
      >
        <VStack align="flex-start">
          {options.map((opt, i) => (
            <Radio value={opt} key={i}>
              {opt}
            </Radio>
          ))}
        </VStack>
      </RadioGroup>
    </Box>
  );
}

function QuestionDisplay({
  question,
  answer,
  onAnswerChange = () => undefined,
}: {
  question: QuizCreationQuestionRequestDto;
  answer?: AnswerSubmitRequestDto;
  onAnswerChange?: (newAnswer: AnswerSubmitRequestDto) => void;
}) {
  let Controls;
  switch (question.questionType) {
    case "singleChoiceQuestion":
    default:
      Controls = SingleChoiceControls;
      break;
  }

  return (
    <>
      <Heading textAlign="center">{question.questionText}</Heading>
      <Controls
        parameters={question.questionParameters}
        answer={answer}
        onAnswerChange={(newAnswer) =>
          onAnswerChange({ ...newAnswer, questionId: question.id ?? 0 })
        }
      />
    </>
  );
}

function SoloGame({ quiz }: { quiz: QuizCreationRequestDto }) {
  const [answers, setAnswers] = useState<AnswerSubmitRequestDto[]>([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);

  const answer = answers.find(
    (a) => a.questionId === quiz.questions[currentQuestionIndex].id
  );

  return (
    <Container maxWidth="3xl">
      <VStack>
        <Progress
          width="100%"
          value={((currentQuestionIndex + 1) * 100) / quiz.questions.length}
        />
        <Flex width="100%">
          <IconButton
            isDisabled={currentQuestionIndex === 0}
            onClick={() => setCurrentQuestionIndex(currentQuestionIndex - 1)}
            aria-label="Previous question"
            flex="1"
            variant="ghost"
            icon={<ChevronLeftIcon />}
          />
          <IconButton
            aria-label="Submit"
            flex="1"
            variant="ghost"
            icon={<CheckIcon />}
          />
          <IconButton
            isDisabled={currentQuestionIndex + 1 === quiz.questions.length}
            onClick={() => setCurrentQuestionIndex(currentQuestionIndex + 1)}
            aria-label="Next question"
            flex="1"
            variant="ghost"
            icon={<ChevronRightIcon />}
          />
        </Flex>
        <QuestionDisplay
          question={quiz.questions[currentQuestionIndex]}
          answer={answer}
          onAnswerChange={(newAnswer) => {
            setAnswers([
              ...answers.filter((a) => a.questionId !== newAnswer.questionId),
              newAnswer,
            ]);
          }}
        />
      </VStack>
    </Container>
  );
}

export default SoloGame;
