import {
  Flex,
  IconButton,
  useColorMode,
  Heading,
  HStack,
  Button,
  Text,
  Popover,
  PopoverTrigger,
  PopoverArrow,
  PopoverBody,
  PopoverCloseButton,
  PopoverContent,
  PopoverFooter,
  PopoverHeader,
  Portal,
  FormControl,
  FormLabel,
  Input,
  VStack,
  useDisclosure,
} from "@chakra-ui/react";
import { MoonIcon, SunIcon } from "@chakra-ui/icons";
import { Link as ReactRouterLink, useMatch } from "react-router-dom";
import { useTheme } from "@chakra-ui/react";
import { AuthContext } from "../contexts/AuthContext";
import { useContext, useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createFlashcardCollection } from "../api/flashcardCollections";
import { FlashcardCollectionDto } from "../types/flashcard";

function DarkModeToggle() {
  const { colorMode, toggleColorMode } = useColorMode();

  return (
    <IconButton
      aria-label="Toggle dark mode"
      onClick={toggleColorMode}
      icon={colorMode === "light" ? <MoonIcon /> : <SunIcon />}
    />
  );
}

function Header() {
  const theme = useTheme();
  const [authInfo, setAuthInfo] = useContext(AuthContext);
  const match = useMatch("/flashcard-collections/*");

  return (
    <Flex
      minHeight="4.5rem"
      alignItems="center"
      position="sticky"
      top={0}
      justifyContent="space-between"
      zIndex="1000"
      bg={theme.styles.global.body.bg}
    >
      <HStack gap="2rem">
        <ReactRouterLink to="/">
          <Heading>Quiz App</Heading>
        </ReactRouterLink>
        <NavSection />
      </HStack>
      <HStack>
        <DarkModeToggle />
        {match ? (
          <CreateFlashcardCollectionPoppover />
        ) : (
          <ReactRouterLink to="/createQuiz">
            <Button>Create Quiz</Button>
          </ReactRouterLink>
        )}
        {authInfo ? (
          <>
            <Text>Logged in as {authInfo.username}</Text>
            <Button onClick={() => setAuthInfo(null)}>Logout</Button>
          </>
        ) : (
          <ReactRouterLink to="/auth">
            <Button>Auth</Button>
          </ReactRouterLink>
        )}
      </HStack>
    </Flex>
  );
}

function NavSection() {
  return (
    <HStack>
      <ReactRouterLink to="/leaderboard/users">
        <Button variant="link">Leaderboard</Button>
      </ReactRouterLink>
      <ReactRouterLink to="/flashcard-collections">
        <Button variant="link">Flashcards</Button>
      </ReactRouterLink>
    </HStack>
  );
}

function CreateFlashcardCollectionPoppover() {
  const [name, setName] = useState("");
  const { isOpen, onToggle, onClose } = useDisclosure();
  const queryClient = useQueryClient();

  const createFlashcardCollectionMutation = useMutation({
    mutationFn: async (flashcardCollection: FlashcardCollectionDto) => {
      await createFlashcardCollection(flashcardCollection);
      queryClient.invalidateQueries({
        queryKey: ["flashcard-collections"],
      });
    },
  });

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setName("");
    onClose();
    createFlashcardCollectionMutation.mutate({ name });
  }

  return (
    <Popover
      onClose={() => {
        setName("");
        onClose();
      }}
      isOpen={isOpen}
    >
      <PopoverTrigger>
        <Button onClick={onToggle}>Create Flashcards</Button>
      </PopoverTrigger>
      <PopoverContent>
        <PopoverCloseButton />
        <PopoverBody>
          <form onSubmit={handleSubmit}>
            <VStack align="start">
              <FormControl>
                <FormLabel>Flashcard collection name:</FormLabel>
                <Input value={name} onChange={(e) => setName(e.target.value)} />
              </FormControl>
              <Button type="submit">Create</Button>
            </VStack>
          </form>
        </PopoverBody>
      </PopoverContent>
    </Popover>
  );
}

export default Header;
