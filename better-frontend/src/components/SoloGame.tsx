import { useContext, useState } from "react";
import {
  AnswerSubmitRequestDto,
  AnswerSubmitResponseDto,
  QuestionParametersDto,
  QuizManipulationQuestionRequestDto,
  QuizManipulationRequestDto,
} from "../types/quiz";
import {
  Flex,
  Box,
  IconButton,
  Heading,
  VStack,
  RadioGroup,
  Radio,
  Progress,
  Input,
  CheckboxGroup,
  Checkbox,
  ListItem,
  List,
  Divider,
} from "@chakra-ui/react";
import { CheckIcon, ChevronLeftIcon, ChevronRightIcon } from "@chakra-ui/icons";
import { submitAnswers } from "../api/quizzes";
import { AuthContext } from "../contexts/AuthContext";
import QuestionStatView from "./QuestionStatView";

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
  const correctName = answer?.optionName || options.join("-") + "---"; // some weird radio button being already selected

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

function MultipleChoiceControls({
  parameters,
  answer,
  onAnswerChange = () => undefined,
}: {
  parameters: QuestionParametersDto;
  answer?: AnswerSubmitRequestDto;
  onAnswerChange?: (newAnswer: AnswerSubmitRequestDto) => void;
}) {
  const options = parameters.options ?? [];
  const correctNames = answer?.optionNames || [];

  return (
    <Box>
      <CheckboxGroup
        value={correctNames}
        onChange={(v) =>
          onAnswerChange({
            optionNames: v.map((opt) => opt.toString()),
            questionId: 0,
          })
        }
      >
        <VStack align="flex-start">
          {options.map((opt, i) => (
            <Checkbox value={opt} key={i}>
              {opt}
            </Checkbox>
          ))}
        </VStack>
      </CheckboxGroup>
    </Box>
  );
}

function OpenTextControls({
  answer,
  onAnswerChange = () => undefined,
}: {
  parameters: QuestionParametersDto;
  answer?: AnswerSubmitRequestDto;
  onAnswerChange?: (newAnswer: AnswerSubmitRequestDto) => void;
}) {
  const answerText = answer?.answerText || "";

  return (
    <Input
      value={answerText}
      placeholder="Type your answer"
      onChange={(e) =>
        onAnswerChange({ answerText: e.target.value, questionId: 0 })
      }
    />
  );
}

function QuestionDisplay({
  question,
  answer,
  onAnswerChange = () => undefined,
}: {
  question: QuizManipulationQuestionRequestDto;
  answer?: AnswerSubmitRequestDto;
  onAnswerChange?: (newAnswer: AnswerSubmitRequestDto) => void;
}) {
  let Controls;
  switch (question.questionType) {
    case "singleChoiceQuestion":
      Controls = SingleChoiceControls;
      break;
    case "openTextQuestion":
      Controls = OpenTextControls;
      break;
    case "multipleChoiceQuestion":
      Controls = MultipleChoiceControls;
      break;
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

function Results({
  quiz,
  results,
  answers,
}: {
  quiz: QuizManipulationRequestDto;
  results: AnswerSubmitResponseDto;
  answers: AnswerSubmitRequestDto[];
}) {
  if (results.status !== "success") {
    return "Internal error";
  }

  return (
    <>
      <VStack gap="20">
        <Box>
          <Heading textAlign="center">
            Score: {results.score.toFixed(2)}
          </Heading>
          <Heading textAlign="center" size="sm">
            {results.correctlyAnswered} / {quiz.questions.length} answered
            correctly
          </Heading>
        </Box>
        <VStack alignSelf="start" w="full">
          <Heading>Stats</Heading>
          <List w="full">
            {results.questionStats.map((s) => {
              const q = quiz.questions.find((q) => q.id === s.questionId);
              const answer = answers.find((a) => a.questionId === s.questionId);
              if (!q) {
                return "Failed to fetch question";
              }

              return (
                <ListItem key={s.questionId} w="full">
                  <Divider width="100%" m="8" />
                  <QuestionStatView
                    question={q}
                    answer={answer}
                    correct={s.correct}
                  />
                </ListItem>
              );
            })}
          </List>
        </VStack>
      </VStack>
    </>
  );
}

function SoloGame({ quiz }: { quiz: QuizManipulationRequestDto }) {
  const [answers, setAnswers] = useState<AnswerSubmitRequestDto[]>([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [results, setResults] = useState<AnswerSubmitResponseDto | null>(null);
  const [authInfo, setAuthInfo] = useContext(AuthContext);

  const answer = answers.find(
    (a) => a.questionId === quiz.questions[currentQuestionIndex].id
  );

  if (results) {
    return <Results quiz={quiz} results={results} answers={answers} />;
  }

  return (
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
          onClick={async () => {
            const results = await submitAnswers(
              quiz.id ?? 0,
              answers,
              authInfo === null ? null : authInfo.token
            );
            setResults(results);
          }}
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
  );
}

export default SoloGame;
