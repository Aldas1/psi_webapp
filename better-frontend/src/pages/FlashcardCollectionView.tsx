import { useQuery, useQueryClient } from "@tanstack/react-query";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import {
  createFlashcard,
  deleteFlashcardCollection,
  getFlashcardCollection,
  getFlashcards,
  deleteFlashcard,
  updateFlashcard,
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

import {
  DeleteIcon,
  EditIcon,
  ArrowBackIcon,
  CloseIcon,
  CheckIcon,
} from "@chakra-ui/icons";
import FlashcardShufflePlay from "../components/FlashcardShufflePlay";

export default function FlashcardCollectionView() {
  const navigate = useNavigate();
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
  const [inShufflePlay, setInShufflePlay] = useState(false);
  const queryClient = useQueryClient();

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

  if (inShufflePlay) {
    return (
      <VStack align="start">
        {
          <IconButton
            icon={<ArrowBackIcon />}
            aria-label="back"
            onClick={() => setInShufflePlay(false)}
          />
        }
        <FlashcardShufflePlay flashcards={flashcards} />
      </VStack>
    );
  }

  async function removeFlashcard(id: number) {
    await deleteFlashcard(id);
    queryClient.invalidateQueries({
      queryKey: ["flashcards", flashcardCollection.id],
    });
  }

  return (
    <VStack align="start">
      <Heading>{flashcardCollection.name}</Heading>
      <HStack>
        {flashcards.length > 0 && (
          <Button onClick={() => setInShufflePlay(true)}>Shuffle play</Button>
        )}
        <Button
          colorScheme="red"
          onClick={async () => {
            navigate("/flashcard-collections");
            await deleteFlashcardCollection(id);
            queryClient.invalidateQueries({
              queryKey: ["flashcard-collections"],
            });
          }}
        >
          Delete
        </Button>
      </HStack>
      <List w="full" maxH="50rem" overflowY="auto">
        <VStack align="stretch">
          {flashcards.map((f) => (
            <ListItem key={f.id}>
              <FlashcardInPreview flashcard={f} onDelete={removeFlashcard} />
            </ListItem>
          ))}
        </VStack>
      </List>
      <Divider />
      <CreateFlashcardForm flashcardCollectionId={id} />
    </VStack>
  );
}

function FlashcardInPreview({
  flashcard,
  onDelete,
}: {
  flashcard: FlashcardDto;
  onDelete: (id: number) => Promise<void>;
}) {
  const [isHovered, setIsHovered] = useState(false);
  const [editing, setEditing] = useState(false);
  const queryClient = useQueryClient();

  function handleEditorExit() {
    setEditing(false);
  }

  async function handleEdit(newFlashcard: FlashcardDto) {
    setEditing(false);
    if (flashcard.id === undefined) return;
    await updateFlashcard(flashcard.id, newFlashcard);
    queryClient.invalidateQueries({
      queryKey: ["flashcards"],
    });
  }

  return (
    <Card
      w="full"
      onMouseOver={() => setIsHovered(true)}
      onMouseOut={() => setIsHovered(false)}
    >
      <CardBody>
        {editing ? (
          <FlashcardEditor
            flashcard={flashcard}
            onExit={handleEditorExit}
            onEdit={handleEdit}
          />
        ) : (
          <HStack justify="space-between">
            <Text>{flashcard.question}</Text>
            <HStack sx={{ visibility: isHovered ? "visible" : "hidden" }}>
              <IconButton
                aria-label="edit"
                icon={<EditIcon />}
                variant="ghost"
                onClick={() => setEditing(true)}
              />
              <IconButton
                aria-label="delete"
                icon={<DeleteIcon />}
                variant="ghost"
                onClick={async () => {
                  if (flashcard.id !== undefined) {
                    await onDelete(flashcard.id);
                  }
                }}
              />
            </HStack>
          </HStack>
        )}
      </CardBody>
    </Card>
  );
}

function FlashcardEditor({
  flashcard,
  onEdit,
  onExit,
}: {
  flashcard: FlashcardDto;
  onEdit: (flashcard: FlashcardDto) => Promise<void>;
  onExit: () => void;
}) {
  const [question, setQuestion] = useState(flashcard.question);
  const [answer, setAnswer] = useState(flashcard.answer);

  return (
    <HStack>
      <Input value={question} onChange={(e) => setQuestion(e.target.value)} />
      <Input value={answer} onChange={(e) => setAnswer(e.target.value)} />
      <IconButton
        icon={<CheckIcon />}
        aria-label="submit"
        onClick={() => onEdit({ question, answer })}
      />
      <IconButton icon={<CloseIcon />} aria-label="close" onClick={onExit} />
    </HStack>
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
        setQuestion("");
        setAnswer("");
      }
    }
    post();
  }, [submitting, question, answer, flashcardCollectionId, queryClient]);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setSubmitting(true);
  }

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
            <Button
              type="submit"
              isDisabled={!question || !answer || submitting}
            >
              Add
            </Button>
          </VStack>
        </form>
      </CardBody>
    </Card>
  );
}
