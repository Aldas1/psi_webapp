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
import { AddIcon, ChevronDownIcon } from "@chakra-ui/icons";
import { QuestionTypeDto } from "../types";

function createQuestionParameters(
  type: QuestionTypeDto
): QuestionParametersDto {
  switch (type) {
    case "singleChoiceQuestion":
    default:
      return {
        options: ["Sample option"],
        correctOptionIndex: 0,
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
        value={correctOptionIndex?.toString()}
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
  onSubmit = () => undefined,
}: {
  quiz: QuizCreationRequestDto;
  onQuizChange?: (newQuiz: QuizCreationRequestDto) => void;
  preview?: boolean;
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
          <Heading size="md" as="h4">
            Questions
          </Heading>
          <Accordion width="100%">
            {quiz.questions.map((q, i) => (
              <AccordionItem key={i}>
                <AccordionButton>
                  <Box flex="1" textAlign="left">
                    Question #{i + 1}
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
