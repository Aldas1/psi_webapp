import {
  Container,
  HStack,
  Heading,
  VStack,
  Box,
  Text,
} from "@chakra-ui/layout";
import {
  QuestionParametersDto,
  QuizCreationQuestionRequestDto,
  QuizCreationRequestDto,
} from "../types";
import { Input } from "@chakra-ui/input";
import { Button, IconButton } from "@chakra-ui/button";
import {
  Accordion,
  AccordionButton,
  AccordionIcon,
  AccordionItem,
  AccordionPanel,
  Menu,
  MenuList,
  MenuItem,
  MenuButton,
  Flex,
  RadioGroup,
  Radio,
} from "@chakra-ui/react";
import { AddIcon, ChevronDownIcon, DeleteIcon } from "@chakra-ui/icons";
import { QuestionTypeDto } from "../types";
import { ReactNode } from "react";

function createQuestionParameters(
  type: QuestionTypeDto
): QuestionParametersDto {
  switch (type) {
    case "singleChoiceQuestion":
      return {
        options: ["Sample option"],
        correctOptionIndex: 0,
      };
    case "openTextQuestion":
    default:
      return {
        correctText: "",
      };
  }
}

function formatQuestionType(type: QuestionTypeDto) {
  switch (type) {
    case "singleChoiceQuestion":
      return "Single choice";
    case "multipleChoiceQuestion":
      return "Multiple choice";
    case "openTextQuestion":
      return "Open text";
  }
}

function SingleChoiceQuestionEditor({
  parameters,
  onParametersChange = () => undefined,
  preview = false,
}: {
  parameters: QuestionParametersDto;
  onParametersChange?: (newParameters: QuestionParametersDto) => void;
  preview?: boolean;
}) {
  const options = parameters.options ?? [];
  const correctOptionIndex = parameters.correctOptionIndex ?? 0;
  return (
    <>
      <RadioGroup
        value={
          preview ? options.join("-") + "-" : correctOptionIndex?.toString()
        }
        isDisabled={preview}
        onChange={(v) =>
          onParametersChange({
            ...parameters,
            correctOptionIndex: parseInt(v),
          })
        }
      >
        <VStack align="start">
          {options.map((o, i) => (
            <HStack key={i}>
              <Radio value={i.toString()}></Radio>
              {preview ? (
                <Text>{o}</Text>
              ) : (
                <>
                  <Input
                    maxLength={30}
                    value={o}
                    placeholder="Option"
                    onChange={(e) =>
                      onParametersChange({
                        ...parameters,
                        options: options.map((opt, optI) =>
                          optI === i ? e.target.value : opt
                        ),
                      })
                    }
                  />
                  <IconButton
                    aria-label="Delete option"
                    icon={<DeleteIcon />}
                    onClick={() => {
                      onParametersChange({
                        ...parameters,
                        options: options.filter((_opt, optI) => optI !== i),
                        correctOptionIndex: 0,
                      });
                    }}
                  />
                </>
              )}
            </HStack>
          ))}
        </VStack>
      </RadioGroup>
      {!preview && (
        <IconButton
          aria-label="Add option"
          variant="ghost"
          icon={<AddIcon />}
          onClick={() => {
            onParametersChange({
              ...parameters,
              options: [...options, ""],
            });
          }}
        />
      )}
    </>
  );
}

function OpenTextQuestionEditor({
  parameters,
  onParametersChange = () => undefined,
  preview = false,
}: {
  parameters: QuestionParametersDto;
  onParametersChange?: (newParameters: QuestionParametersDto) => void;
  preview?: boolean;
}) {
  const value = parameters.correctText ?? "";

  if (preview) return "";

  return (
    <Input
      maxLength={40}
      placeholder="Correct answer"
      value={value}
      onChange={(e) =>
        onParametersChange({ ...parameters, correctText: e.target.value })
      }
    />
  );
}

function QuestionEditor({
  question,
  onQuestionChange = () => undefined,
  preview = false,
}: {
  question: QuizCreationQuestionRequestDto;
  onQuestionChange?: (newQuestion: QuizCreationQuestionRequestDto) => void;
  preview?: boolean;
}) {
  let ParametersEditor;
  switch (question.questionType) {
    case "singleChoiceQuestion":
      ParametersEditor = SingleChoiceQuestionEditor;
      break;
    case "openTextQuestion":
      ParametersEditor = OpenTextQuestionEditor;
      break;
    default:
      ParametersEditor = SingleChoiceQuestionEditor;
      break;
  }

  return (
    <VStack width="100%" align="flex-start">
      {preview ? (
        <>
          <Heading size="sm" as="h5">
            {question.questionText}
          </Heading>
          <Text fontSize="xs" fontStyle="italic">
            {formatQuestionType(question.questionType)}
          </Text>
        </>
      ) : (
        <Flex gap="1rem" width="100%">
          <Input
            flex="1"
            maxLength={80}
            width="100%"
            value={question.questionText}
            placeholder="Question text"
            onChange={(e) =>
              onQuestionChange({ ...question, questionText: e.target.value })
            }
          />
          <Menu>
            <MenuButton as={Button} rightIcon={<ChevronDownIcon />}>
              Type
            </MenuButton>
            <MenuList>
              <MenuItem
                onClick={() =>
                  onQuestionChange({
                    ...question,
                    questionType: "singleChoiceQuestion",
                    questionParameters: createQuestionParameters(
                      "singleChoiceQuestion"
                    ),
                  })
                }
              >
                Single choice question
              </MenuItem>
              <MenuItem
                onClick={() =>
                  onQuestionChange({
                    ...question,
                    questionType: "openTextQuestion",
                    questionParameters:
                      createQuestionParameters("openTextQuestion"),
                  })
                }
              >
                Open text question
              </MenuItem>
            </MenuList>
          </Menu>
        </Flex>
      )}
      <ParametersEditor
        parameters={question.questionParameters}
        onParametersChange={(newParameters) =>
          onQuestionChange({ ...question, questionParameters: newParameters })
        }
        preview={preview}
      />
    </VStack>
  );
}

function QuizEditor({
  quiz,
  onQuizChange = () => undefined,
  preview = false,
  previewBody,
  onSubmit = () => undefined,
}: {
  quiz: QuizCreationRequestDto;
  onQuizChange?: (newQuiz: QuizCreationRequestDto) => void;
  preview?: boolean;
  previewBody?: ReactNode;
  onSubmit?: () => void;
}) {
  return (
    <Container maxWidth="3xl">
      <form
        onSubmit={(e) => {
          e.preventDefault();
          onSubmit();
        }}
      >
        <VStack align="flex-start">
          {preview ? (
            <Heading size="lg" as="h3">
              {quiz.name}
            </Heading>
          ) : (
            <Input
              required
              value={quiz.name}
              onChange={(e) => onQuizChange({ ...quiz, name: e.target.value })}
              maxLength={80}
              placeholder="Quiz name"
              width="100%"
            />
          )}
          {preview && previewBody}
          <Heading size="md" as="h4">
            Questions
          </Heading>
          <Accordion width="100%">
            {quiz.questions.map((q, i) => (
              <AccordionItem key={i}>
                <AccordionButton>
                  <Box flex="1" textAlign="left">
                    Question #{i + 1}{" "}
                    {!preview && ` (${formatQuestionType(q.questionType)})`}
                  </Box>
                  <AccordionIcon />
                </AccordionButton>
                <AccordionPanel>
                  <VStack align="start">
                    <QuestionEditor
                      question={q}
                      preview={preview}
                      onQuestionChange={(newQuestion) =>
                        onQuizChange({
                          ...quiz,
                          questions: quiz.questions.map((question) =>
                            question === q ? newQuestion : question
                          ),
                        })
                      }
                    />
                    {!preview && (
                      <Button
                        onClick={() => {
                          onQuizChange({
                            ...quiz,
                            questions: quiz.questions.filter(
                              (question) => q !== question
                            ),
                          });
                        }}
                      >
                        Remove
                      </Button>
                    )}
                  </VStack>
                </AccordionPanel>
              </AccordionItem>
            ))}
          </Accordion>
          {!preview && (
            <HStack>
              <Button
                onClick={() =>
                  onQuizChange({
                    ...quiz,
                    questions: [
                      ...quiz.questions,
                      {
                        questionText: "",
                        questionType: "singleChoiceQuestion",
                        questionParameters: createQuestionParameters(
                          "singleChoiceQuestion"
                        ),
                      },
                    ],
                  })
                }
              >
                Add question
              </Button>
              <Button type="submit">Submit</Button>
            </HStack>
          )}
        </VStack>
      </form>
    </Container>
  );
}

export default QuizEditor;
