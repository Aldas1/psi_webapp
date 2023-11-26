import { useQuery } from "@tanstack/react-query";
import { Navigate, useParams } from "react-router-dom";
import {
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
} from "@chakra-ui/react";
import { FlashcardDto } from "../types/flashcard";
import { useState } from "react";

import { DeleteIcon, EditIcon, AddIcon } from "@chakra-ui/icons";

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
      <List w="full">
        <VStack align="stretch">
          {flashcards.map((f) => (
            <ListItem key={f.id}>
              <FlashcardInPreview flashcard={f} />
            </ListItem>
          ))}
        </VStack>
      </List>
      <IconButton icon={<AddIcon />} aria-label="Add flashcard" />
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
