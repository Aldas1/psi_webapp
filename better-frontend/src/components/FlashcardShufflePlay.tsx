import { useEffect, useState } from "react";
import { FlashcardDto } from "../types/flashcard";
import {
  Box,
  Button,
  Center,
  Flex,
  HStack,
  Heading,
  IconButton,
  Text,
  VStack,
} from "@chakra-ui/react";

function shuffle<T>(arr: T[]): T[] {
  return Array.from(arr).sort(() => Math.random() - 0.5);
}

export default function FlashcardShufflePlay({
  flashcards,
}: {
  flashcards: FlashcardDto[];
}) {
  const [leftFlashcards, setLeftFlashcards] = useState(shuffle(flashcards));
  const [nextLeftFlashcards, setNextLeftFlashcards] = useState<FlashcardDto[]>(
    []
  );

  useEffect(() => {
    if (leftFlashcards.length === 0 && nextLeftFlashcards.length > 0) {
      setLeftFlashcards(shuffle(nextLeftFlashcards));
      setNextLeftFlashcards([]);
    }
  }, [leftFlashcards, nextLeftFlashcards]);

  function remember() {
    setLeftFlashcards(leftFlashcards.filter((_v, i) => i !== 0));
  }

  function repeat() {
    setLeftFlashcards(leftFlashcards.filter((_v, i) => i !== 0));
    setNextLeftFlashcards([...nextLeftFlashcards, leftFlashcards[0]]);
  }

  if (leftFlashcards.length === 0 && nextLeftFlashcards.length === 0) {
    return (
      <>
        <Heading>Congrats!</Heading>
        <Button onClick={() => setLeftFlashcards(shuffle(flashcards))}>
          Repeat
        </Button>
      </>
    );
  }

  if (leftFlashcards.length > 0) {
    return (
      <>
        <Flashcard flashcard={leftFlashcards[0]} />
        <HStack justifyContent="center" mx="auto">
          <Button onClick={remember}>Remeber</Button>
          <Button onClick={repeat}>Repeat</Button>
        </HStack>
      </>
    );
  }
}

function Flashcard({ flashcard }: { flashcard: FlashcardDto }) {
  const [isFlipped, setIsFlipped] = useState(false);

  return (
    <VStack
      backgroundColor="blackAlpha.400"
      w="full"
      aspectRatio="16/9"
      justify="center"
      rounded="2xl"
      transition="0.5s"
      transform={isFlipped ? "rotateY(180deg)" : ""}
      onClick={() => setIsFlipped(!isFlipped)}
      userSelect="none"
    >
      <Text fontSize="4xl" transform={isFlipped ? "rotateY(180deg)" : ""}>
        {isFlipped ? flashcard.answer : flashcard.question}
      </Text>
    </VStack>
  );
}
