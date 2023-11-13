import { useEffect, useState } from "react";
import DiscussionHub from "../api/discussionHub";
import { DiscussionComment } from "../types";

export default function useDiscussion(id: number) {
  const [discussionHub, setDiscussionHub] = useState(new DiscussionHub());
  const [comments, setComments] = useState<DiscussionComment[]>([]);

  useEffect(() => {
    discussionHub.openConnection();
    console.log("opening connection");
  });

  async function postComment() {}

  return {
    comments,
    postComment,
  };
}
