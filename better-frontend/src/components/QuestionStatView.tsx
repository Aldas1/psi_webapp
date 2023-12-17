import {
  Heading,
  VStack,
  Text,
  HStack,
  ListItem,
  List,
  Checkbox,
} from "@chakra-ui/react";
import {
  AnswerSubmitRequestDto,
  QuizManipulationQuestionRequestDto,
} from "../types/quiz";

interface QuestionStatViewProps {
  question: QuizManipulationQuestionRequestDto;
  answer?: AnswerSubmitRequestDto;
  correct: boolean;
}

export default function QuestionStatView({
  question,
  answer,
  correct,
}: QuestionStatViewProps) {
  let QuestionComponent = SingleChoiceQuestionStatView;
  switch (question.questionType) {
    case "multipleChoiceQuestion":
      QuestionComponent = MultipleChoiceQuestionStatView;
      break;
    case "openTextQuestion":
      QuestionComponent = OpenTextQuestionStatView;
      break;
  }
  return (
    <VStack align="flex-start">
      <HStack>
        <Heading size="md">{question.questionText}</Heading>
        <QuestionCorrectnessBadge correct={correct} />
      </HStack>
      <QuestionComponent
        question={question}
        answer={answer}
        correct={correct}
      />
    </VStack>
  );
}

function QuestionCorrectnessBadge({ correct }: { correct: boolean }) {
  return (
    <Heading size="xs" color={correct ? "" : "red.500"}>
      ({correct ? 1 : 0} / 1)
    </Heading>
  );
}

function SingleChoiceQuestionStatView({
  question,
  answer,
}: QuestionStatViewProps) {
  const correctOptionIndex = question.questionParameters.correctOptionIndex;
  const options = question.questionParameters.options ?? [];
  return (
    <QuestionWithOptionsStatView
      options={options}
      pickedOptions={[answer?.optionName ?? ""]}
      correctOptions={[options[correctOptionIndex ?? 0]]}
    />
  );
}

function MultipleChoiceQuestionStatView({
  question,
  answer,
}: QuestionStatViewProps) {
  const options = question.questionParameters.options ?? [];
  return (
    <QuestionWithOptionsStatView
      options={options}
      pickedOptions={answer?.optionNames ?? []}
      correctOptions={
        question.questionParameters.correctOptionIndexes?.map(
          (i) => options[i]
        ) ?? []
      }
    />
  );
}

function QuestionWithOptionsStatView({
  options,
  correctOptions,
  pickedOptions,
}: {
  options: string[];
  correctOptions: string[];
  pickedOptions: string[];
}) {
  return (
    <List w="full">
      <VStack>
        {options.map((o, i) => {
          let correct = false;
          if (correctOptions.includes(o) === pickedOptions.includes(o))
            correct = true;
          const unpicked = !correct && correctOptions.includes(o);
          return (
            <ListItem
              key={o + i}
              bgColor={
                correct
                  ? pickedOptions.includes(o)
                    ? "green.500"
                    : ""
                  : !unpicked
                  ? "red.500"
                  : ""
              }
              textColor={
                (correct && pickedOptions.includes(o)) ||
                (!correct && !unpicked)
                  ? "white"
                  : ""
              }
              borderColor="red.500"
              borderWidth={unpicked ? 2 : 0}
              p="4"
              rounded="lg"
              w="full"
            >
              <Checkbox
                isChecked={pickedOptions.includes(o)}
                isReadOnly
                cursor="auto"
              >
                {o}
              </Checkbox>
            </ListItem>
          );
        })}
      </VStack>
    </List>
  );
}

function OpenTextQuestionStatView({
  question,
  answer,
  correct,
}: QuestionStatViewProps) {
  if (correct) {
    return <Text>{answer?.answerText}</Text>;
  } else {
    return (
      <Text>
        <Text as="s">{answer?.answerText}</Text>
        <Text as="span"> ({question.questionParameters.correctText})</Text>
      </Text>
    );
  }
}
