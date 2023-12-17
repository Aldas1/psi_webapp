import {
  Box,
  Button,
  Container,
  HStack,
  Input,
  List,
  ListItem,
  Text,
} from "@chakra-ui/react";
import useDiscussion from "../hooks/useDiscussion";
import { DiscussionComment } from "../types/quiz";
import { useEffect, useRef, useState } from "react";

export default function QuizDiscussionBlock({ id }: { id: number }) {
  const { comments, postComment } = useDiscussion(id);

  return (
    <Container>
      {comments !== null ? (
        <CommentArea comments={comments} />
      ) : (
        <Box>Loading...</Box>
      )}
      <PostComment postComment={postComment} />
    </Container>
  );
}

function PostComment({
  postComment,
}: {
  postComment: (content: string) => Promise<void>;
}) {
  const [inpValue, setInpValue] = useState("");

  async function post() {
    if (inpValue.trim().length < 1) return;
    setInpValue("");
    await postComment(inpValue);
  }

  return (
    <HStack>
      <Input
        value={inpValue}
        onChange={(e) => setInpValue(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === "Enter") {
            post();
          }
        }}
      />
      <Button onClick={post}>Send</Button>
    </HStack>
  );
}

function CommentArea({ comments }: { comments: DiscussionComment[] }) {
  const sortedComments = comments.sort(
    (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
  );

  const messagesEnd = useRef<HTMLDivElement>(null);

  const scrollToBottom = () => {
    messagesEnd.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [comments]);

  return (
    <List height="83vh" overflowX="hidden" overflowY="scroll">
      {sortedComments.map((c, i) => {
        return (
          <ListItem key={i}>
            <Text as="span" fontWeight="bold" color="purple.300">
              {c.username == null ? "Guest" : c.username}
            </Text>
            : {c.content}
          </ListItem>
        );
      })}
      <div ref={messagesEnd}></div>
    </List>
  );
}
