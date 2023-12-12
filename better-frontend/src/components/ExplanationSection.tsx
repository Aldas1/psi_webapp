import { Button, VStack, Text } from "@chakra-ui/react";
import { useState } from "react";
import { getExplanation } from "../api/questions";

export default function ExplanationSection({
  quizId,
  questionId,
}: {
  quizId: number;
  questionId: number;
}) {
  const [explanation, setExplanation] = useState<string | undefined>(undefined);

  async function generateExplanation() {
    setExplanation("Loading...");
    const data = await getExplanation(quizId, questionId);
    if (!data.explanation) {
      setExplanation("Failed to generate explanation, try again");
    } else {
      setExplanation(data.explanation);
    }
  }

  return (
    <VStack align="self-start">
      <Button onClick={generateExplanation}>Explain</Button>
      <Text>{explanation}</Text>
    </VStack>
  );
}
