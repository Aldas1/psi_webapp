import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router";
import {
  deleteQuiz,
  getQuiz,
  updateQuiz,
} from "../api/quizzes";
import {
  getQuestions
} from "../api/questions";
import { Button, HStack, Spinner, useToast } from "@chakra-ui/react";
import {
  QuestionResponseDto,
  QuizManipulationRequestDto,
  QuizResponseDto,
} from "../types";
import QuizEditor from "../components/QuizEditor";
import { useState } from "react";
import SoloGame from "../components/SoloGame";

function generateQuiz(
  quizResponse: QuizResponseDto,
  questionsResponse: QuestionResponseDto[]
): QuizManipulationRequestDto {
  return {
    id: quizResponse.id,
    name: quizResponse.name,
    questions: questionsResponse,
  };
}

function QuizPreview() {
  const toast = useToast();
  const params = useParams();
  const id = parseInt(params.id ?? "0");

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
            <Button onClick={() => setInGame(true)}>Solo game</Button>
          )}
          <Button colorScheme="purple" onClick={() => setQuizForEdit(quiz)}>
            Edit quiz
          </Button>
          <Button
            colorScheme="red"
            onClick={async () => {
              await deleteQuiz(id);
              navigate("/");
            }}
          >
            Delete
          </Button>
        </HStack>
      }
    />
  );
}

export default QuizPreview;
