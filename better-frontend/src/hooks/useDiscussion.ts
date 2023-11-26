import { useContext, useEffect, useState } from "react";
import { DiscussionComment } from "../types/quiz";
import { AuthContext } from "../contexts/AuthContext";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const connectionUrl = "https://localhost:7280/discussionHub";

export default function useDiscussion(id: number) {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [connected, setConnected] = useState(false);
  const [userInfo] = useContext(AuthContext);
  const [comments, setComments] = useState<DiscussionComment[]>([]);

  useEffect(() => {
    if (connection !== null) return;
    const newConnection = new HubConnectionBuilder()
      .withUrl(connectionUrl)
      .build();

    setConnection(newConnection);
  }, [connection]);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("SignalR Connected");
          setConnected(true);
          connection.invoke("AddToGroup", id);
        })
        .catch((err) => console.error("SignalR Connection Error: ", err));
    }

    return () => {
      if (connection) {
        connection
          .stop()
          .then(() => {
            console.log("SignalR Connection Stopped");
            setConnected(false);
          })
          .catch((err) => console.error("SignalR Stop Error: ", err));
      }
    };
  }, [connection, id]);

  useEffect(() => {
    if (connection === null) return;
    connection.on("NewMessage", (newMessage: DiscussionComment) => {
      setComments((c) => [...c, newMessage]);
    });

    connection.on("NewMessages", (newMessages: DiscussionComment[]) => {
      console.log("got new messages");
      console.log(newMessages);
      setComments((c) => c.concat(newMessages));
    });

    return () => {
      connection.off("NewMessage");
      connection.off("NewMessages");
    };
  }, [connection, comments]);

  async function postComment(content: string) {
    if (!connected) return;
    const username = userInfo == null ? null : userInfo.username;
    connection?.invoke("PostComment", username, content, id);
  }

  return {
    comments,
    postComment,
  };
}
