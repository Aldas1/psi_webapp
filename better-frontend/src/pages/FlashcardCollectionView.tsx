import { useQuery, useQueryClient } from "@tanstack/react-query";
import { Navigate, useParams } from "react-router-dom";
import {
  createFlashcard,
  getFlashcardCollection,
  getFlashcards,
} from "../api/flashcardCollections";
import {
  Card,
  CardBody,
  HStack,
  Heading,
  List,
  ListItem,
  Spinner,
  VStack,
  Text,
  IconButton,
  FormControl,
  FormLabel,
  Divider,
  CardHeader,
  Input,
  Button,
} from "@chakra-ui/react";
import { FlashcardDto } from "../types/flashcard";
import React, { useEffect, useState } from "react";

import { DeleteIcon, EditIcon } from "@chakra-ui/icons";

export default function FlashcardCollectionView() {
  const params = useParams();
  const id = parseInt(params.id ?? "0");
  const flashcardCollectionQuery = useQuery({
    queryKey: ["flashcard-collections", id],
    queryFn: async () => await getFlashcardCollection(id),
  });
  const flashcardQuery = useQuery({
    queryKey: ["flashcards", id],
    queryFn: async () => getFlashcards(id),
  });

  if (id === null) return <Navigate to="not-found" />;
  if (flashcardCollectionQuery.isLoading || flashcardQuery.isLoading)
    return <Spinner />;
  if (
    flashcardCollectionQuery.isError ||
    !flashcardCollectionQuery.data ||
    flashcardQuery.isError ||
    !flashcardQuery.data
  )
    return "Unexpected error";

  const flashcardCollection = flashcardCollectionQuery.data;
  const flashcards = flashcardQuery.data;

  return (
    <VStack align="start">
      <Heading>{flashcardCollection.name}</Heading>
      <HStack>
        <div>stuff here</div>
      </HStack>
      <List w="full" maxH="50rem" overflowY="auto">
        <VStack align="stretch">
          {flashcards.map((f) => (
            <ListItem key={f.id}>
              <FlashcardInPreview flashcard={f} />
            </ListItem>
          ))}
        </VStack>
      </List>
      <Divider />
      <CreateFlashcardForm flashcardCollectionId={id} />
    </VStack>
  );
}

function FlashcardInPreview({ flashcard }: { flashcard: FlashcardDto }) {
  const [isHovered, setIsHovered] = useState(false);
  const [editing, setEditing] = useState(false);
  return (
    <Card
      w="full"
      onMouseOver={() => setIsHovered(true)}
      onMouseOut={() => setIsHovered(false)}
    >
      <CardBody>
        <HStack justify="space-between">
          <Text>{flashcard.question}</Text>
          <HStack sx={{ visibility: isHovered ? "visible" : "hidden" }}>
            <IconButton aria-label="edit" icon={<EditIcon />} variant="ghost" />
            <IconButton
              aria-label="delete"
              icon={<DeleteIcon />}
              variant="ghost"
            />
          </HStack>
        </HStack>
      </CardBody>
    </Card>
  );
}

function CreateFlashcardForm({
  flashcardCollectionId,
}: {
  flashcardCollectionId: number;
}) {
  const [question, setQuestion] = useState("");
  const [answer, setAnswer] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const queryClient = useQueryClient();

  useEffect(() => {
    async function post() {
      if (submitting) {
        await createFlashcard(flashcardCollectionId, { question, answer });
        queryClient.invalidateQueries({
          queryKey: ["flashcards", flashcardCollectionId],
        });
        setSubmitting(false);
      }
    }
    post();
  }, [submitting, question, answer, flashcardCollectionId, queryClient]);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setSubmitting(true);
  }

  if (submitting) return <Spinner />;

  return (
    <Card w="full">
      <CardHeader>Create new</CardHeader>
      <CardBody>
        <form onSubmit={handleSubmit}>
          <VStack align="start">
            <FormControl display="flex" alignItems="center">
              <FormLabel>Question</FormLabel>
              <Input
                value={question}
                onChange={(e) => setQuestion(e.target.value)}
              />
            </FormControl>
            <FormControl display="flex" alignItems="center">
              <FormLabel>Answer</FormLabel>
              <Input
                value={answer}
                onChange={(e) => setAnswer(e.target.value)}
              />
            </FormControl>
            <Button type="submit" isDisabled={!question || !answer}>
              Add
            </Button>
          </VStack>
        </form>
      </CardBody>
    </Card>
  );
}
