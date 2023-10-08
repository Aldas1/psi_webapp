import { useQuery } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router";
import { deleteQuiz, getQuestions, getQuiz } from "../../api/quizzes";
import { Button, HStack, Spinner } from "@chakra-ui/react";
import {
  QuestionResponseDto,
  QuizCreationRequestDto,
  QuizResponseDto,
} from "../../types";
import QuizEditor from "../../components/QuizEditor";

function generateQuiz(
  quizResponse: QuizResponseDto,
  questionsResponse: QuestionResponseDto[]
): QuizCreationRequestDto {
  return {
    name: quizResponse.name,
    questions: questionsResponse.map((q) => {
      return {
        questionText: q.questionText,
        questionType: q.questionType,
        questionParameters: q.questionParameters,
      };
    }),
  };
}

function QuizPreview() {
  const params = useParams();
  const id = parseInt(params.id ?? "0");

  const navigate = useNavigate();

  const quizQuery = useQuery({
    queryKey: ["quiz", id],
    queryFn: () => getQuiz(id),
  });
  const questionsQuery = useQuery({
    queryKey: ["questions", id],
    queryFn: () => getQuestions(id),
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

  return (
    <QuizEditor
      quiz={quiz}
      preview
      previewBody={
        <HStack>
          <Button>Solo game</Button>
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
