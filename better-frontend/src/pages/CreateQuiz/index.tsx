import { useState } from "react";
import { QuizCreationRequestDto } from "../../types";
import QuizEditor from "../../components/QuizEditor";
import { Heading, VStack } from "@chakra-ui/layout";
import { createQuiz } from "../../api/quizzes";
import { useToast } from "@chakra-ui/toast";

function CreateQuiz() {
  const [quiz, setQuiz] = useState<QuizCreationRequestDto>({
    name: "",
    questions: [],
  });
  const toast = useToast();

  return (
    <VStack>
      <Heading size="lg" alignSelf="right">
        Create quiz
      </Heading>
      <QuizEditor
        quiz={quiz}
        onQuizChange={(newQuiz) => setQuiz(newQuiz)}
        onSubmit={async () => {
          const response = await createQuiz(quiz);
          if (response.status !== "success") {
            toast({
              title: "Quiz creation failed",
              description: response.status,
              status: "error",
              duration: 5000,
              isClosable: true,
            });
          } else {
            toast({
              title: "Success",
              description: "Congrats! Your quiz is created.",
              status: "success",
              duration: 5000,
              isClosable: true,
            });
          }
        }}
      />
    </VStack>
  );
}

export default CreateQuiz;
