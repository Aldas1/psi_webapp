import useDiscussion from "../hooks/useDiscussion";

export default function QuizDiscussionBlock() {
  const { comments, postComment } = useDiscussion(0);

  return "quiz discussion";
}
