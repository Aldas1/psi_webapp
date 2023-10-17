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
import { useState } from "react";
import SoloGame from "../../components/SoloGame";

function generateQuiz(
  quizResponse: QuizResponseDto,
  questionsResponse: QuestionResponseDto[]
): QuizCreationRequestDto {
  return {
    id: quizResponse.id,
    name: quizResponse.name,
    questions: questionsResponse,
  };
}

function QuizPreview() {
  const params = useParams();
  const id = parseInt(params.id ?? "0");

  const [inGame, setInGame] = useState(false);

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

  if (inGame) {
    return <SoloGame quiz={quiz} />;
  }

  return (
    <QuizEditor
      quiz={quiz}
      preview
      previewBody={
        <HStack>
          {quiz.questions.length > 0 && (
            <Button onClick={() => setInGame(true)}>Solo game</Button>
          )}
          <Button
            colorScheme="purple"
            onClick={() => navigate(`/quizzes/${id}`)}
          >
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
