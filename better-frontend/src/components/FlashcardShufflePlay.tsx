import { useState } from "react";
import { FlashcardDto } from "../types/flashcard";
import {
  Button,
  HStack,
  Heading,
  Progress,
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
  const [rememberedFlashcards, setRememberedFlashcards] = useState<
    FlashcardDto[]
  >([]);

  function reset() {
    setNextLeftFlashcards([]);
    setRememberedFlashcards([]);
  }

  function nextRound() {
    setLeftFlashcards(shuffle(nextLeftFlashcards));
    reset();
  }

  function remember() {
    setLeftFlashcards(leftFlashcards.filter((_v, i) => i !== 0));
    setRememberedFlashcards([...rememberedFlashcards, leftFlashcards[0]]);
  }

  function repeat() {
    setLeftFlashcards(leftFlashcards.filter((_v, i) => i !== 0));
    setNextLeftFlashcards([...nextLeftFlashcards, leftFlashcards[0]]);
  }

  if (leftFlashcards.length === 0 && nextLeftFlashcards.length === 0) {
    return (
      <>
        <Heading>Congrats!</Heading>
        <Button
          onClick={() => {
            setLeftFlashcards(shuffle(flashcards));
            reset();
          }}
        >
          Repeat
        </Button>
      </>
    );
  }

  if (leftFlashcards.length === 0 && nextLeftFlashcards.length > 0) {
    return (
      <NextRoundView
        rememberedCount={rememberedFlashcards.length}
        nextCount={nextLeftFlashcards.length}
        onProceed={nextRound}
      />
    );
  }

  if (leftFlashcards.length > 0) {
    return (
      <>
        <Progress
          w="full"
          value={
            ((nextLeftFlashcards.length + rememberedFlashcards.length) * 100) /
            (nextLeftFlashcards.length +
              leftFlashcards.length +
              rememberedFlashcards.length)
          }
        />
        <Flashcard flashcard={leftFlashcards[0]} />
        <HStack justifyContent="center" mx="auto">
          <Button onClick={remember}>Remeber</Button>
          <Button onClick={repeat}>Repeat</Button>
        </HStack>
      </>
    );
  }
}

function NextRoundView({
  rememberedCount,
  nextCount,
  onProceed,
}: {
  rememberedCount: number;
  nextCount: number;
  onProceed: () => void;
}) {
  return (
    <VStack w="full">
      <Heading size="lg">Your progress so far</Heading>
      <Text>
        {rememberedCount} remembered, {nextCount} left
      </Text>
      <Button onClick={onProceed}>Continue</Button>
    </VStack>
  );
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
