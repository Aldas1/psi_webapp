import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router";
import { deleteQuiz, getQuiz, updateQuiz } from "../api/quizzes";
import { getQuestions } from "../api/questions";
import {
  Button,
  HStack,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalHeader,
  ModalOverlay,
  Spinner,
  useDisclosure,
  useToast,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  IconButton,
  Drawer,
  DrawerBody,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
} from "@chakra-ui/react";
import { ChatIcon } from "@chakra-ui/icons";
import {
  QuestionResponseDto,
  QuizManipulationRequestDto,
  QuizResponseDto,
} from "../types/quiz";
import QuizEditor from "../components/QuizEditor";
import { useContext, useState } from "react";
import SoloGame from "../components/SoloGame";
import QuizDiscussionBlock from "../components/QuizDiscussionBlock";
import { AuthContext } from "../contexts/AuthContext";
import { createFromQuizFlashcardCollection } from "../api/flashcardCollections";

function generateQuiz(
  quizResponse: QuizResponseDto,
  questionsResponse: QuestionResponseDto[]
): QuizManipulationRequestDto {
  return {
    id: quizResponse.id,
    name: quizResponse.name,
    questions: questionsResponse,
    owner: quizResponse.owner,
  };
}

function QuizPreview() {
  const toast = useToast();
  const params = useParams();
  const id = parseInt(params.id ?? "0");
  const [authInfo] = useContext(AuthContext);

  const [inGame, setInGame] = useState(false);
  const [quizForEdit, setQuizForEdit] =
    useState<QuizManipulationRequestDto | null>(null);

  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const quizQuery = useQuery({
    queryKey: ["quiz", id],
    queryFn: () => getQuiz(id),
  });
  const questionsQuery = useQuery({
    queryKey: ["questions", id],
    queryFn: () => getQuestions(id),
  });

  const quizMutation = useMutation({
    mutationFn: async (newQuiz: QuizManipulationRequestDto) => {
      await updateQuiz(id, newQuiz);
      queryClient.invalidateQueries(["quiz", id]);
      queryClient.invalidateQueries(["questions", id]);
    },
    onError: (err: Error) => {
      toast({
        title: "Quiz edit failed",
        description: `Whoops! ${err.message}`,
        status: "error",
        duration: 5000,
        isClosable: true,
      });
    },
    onSuccess: () => {
      toast({
        title: "Quiz edit success",
        description: `Congrats! Go solve it!`,
        status: "success",
        duration: 5000,
        isClosable: true,
      });
      setQuizForEdit(null);
    },
  });

  const quizData = quizQuery.data;
  const quizIsLoading = quizQuery.isLoading;
  const quizIsError = quizQuery.isError;
  const questionsData = questionsQuery.data;
  const questionsIsLoading = questionsQuery.isLoading;
  const questionsIsError = questionsQuery.isError;
  const [generateButtonIsLoading, setGenerateButtonIsLoading] = useState(false);

  const { isOpen, onOpen, onClose } = useDisclosure();

  if (quizIsLoading || questionsIsLoading) {
    return <Spinner />;
  }

  if (
    quizIsError ||
    questionsIsError ||
    quizData === undefined ||
    questionsData === undefined
  ) {
    return "Unexpected Error!";
  }

  const quiz = generateQuiz(quizData, questionsData);

  if (inGame) {
    return <SoloGame quiz={quiz} />;
  }

  return (
    <>
      <QuizEditor
        quiz={quizForEdit ?? quiz}
        preview={quizForEdit === null}
        onQuizChange={(newQuiz) => setQuizForEdit(newQuiz)}
        onSubmit={() => {
          if (quizForEdit) {
            quizMutation.mutate(quizForEdit);
          }
        }}
        previewBody={
          <HStack>
            {quiz.questions.length > 0 && (
              <Button colorScheme="green" onClick={() => setInGame(true)}>
                Start
              </Button>
            )}
            {(quiz.owner === undefined ||
              quiz.owner === authInfo?.username) && (
              <>
                <Menu>
                  <MenuButton as={Button} colorScheme="purple">
                    Actions
                  </MenuButton>
                  <MenuList>
                    {quiz.questions.length > 0 && (
                      <MenuItem
                        as={Button}
                        onClick={async () => {
                          if (generateButtonIsLoading) {
                            return;
                          }

                          setGenerateButtonIsLoading(true);

                          try {
                            const newCollection =
                              await createFromQuizFlashcardCollection(id);
                            navigate(
                              `/flashcard-collections/${newCollection.id}`
                            );
                          } catch (error) {
                            toast({
                              title: "Flashcard generation failed",
                              description: `Whoops! ${
                                (error as Error).message
                              }`,
                              status: "error",
                              duration: 5000,
                              isClosable: true,
                            });
                          }

                          setGenerateButtonIsLoading(false);
                        }}
                        isDisabled={generateButtonIsLoading}
                      >
                        {generateButtonIsLoading
                          ? "Generating..."
                          : "Generate flashcards"}
                      </MenuItem>
                    )}
                    <MenuItem
                      as={Button}
                      textAlign="center"
                      onClick={() => setQuizForEdit(quiz)}
                    >
                      Edit
                    </MenuItem>
                    <MenuItem
                      as={Button}
                      colorScheme="red"
                      onClick={async () => {
                        await deleteQuiz(id);
                        navigate("/");
                      }}
                    >
                      Delete
                    </MenuItem>
                  </MenuList>
                </Menu>
              </>
            )}
            <IconButton
              variant="outline"
              aria-label="Discussion"
              icon={<ChatIcon />}
              onClick={onOpen}
            />
          </HStack>
        }
      />
      <Drawer isOpen={isOpen} placement="right" onClose={onClose} size="md">
        <DrawerOverlay />
        <DrawerContent>
          <DrawerHeader>Quiz discussion</DrawerHeader>
          <DrawerCloseButton />
          <DrawerBody display="flex" alignItems="stretch">
            <QuizDiscussionBlock id={quiz.id} />
          </DrawerBody>
        </DrawerContent>
      </Drawer>
    </>
  );
}

export default QuizPreview;
