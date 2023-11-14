import {
  Box,
  Button,
  Container,
  HStack,
  Input,
  List,
  ListItem,
} from "@chakra-ui/react";
import useDiscussion from "../hooks/useDiscussion";
import { DiscussionComment } from "../types";
import { useState } from "react";

export default function QuizDiscussionBlock({ id }: { id: number }) {
  const { comments, postComment } = useDiscussion(id);

  return (
    <Container display="flex" flexDirection="column">
      {comments !== null ? (
        <CommentArea comments={comments} />
      ) : (
        <Box flex="9">Loading...</Box>
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
    <HStack flex="1">
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

  return (
    <List flex="9">
      {sortedComments.map((c, i) => {
        return (
          <ListItem key={i}>
            {c.username == null ? "Guest" : c.username}: {c.content}
          </ListItem>
        );
      })}
    </List>
  );
}
