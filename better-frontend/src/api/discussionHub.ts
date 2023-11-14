import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const connectionUrl = "https://localhost:7280/discussionHub";

class DiscussionHub {
  connection: HubConnection | null;

  constructor() {
    this.connection = null;
  }

  async closeConnection() {
    await this.connection?.stop();
  }

  async openConnection() {
    this.connection = new HubConnectionBuilder().withUrl(connectionUrl).build();
    await this.connection.start();
  }

  async postComment(content: string) {
    await this.connection?.invoke("PostComment", content);
  }

  async addToGroup(quizId: number) {
    await this.connection?.invoke("AddToGroup", quizId);
  }
}

export default DiscussionHub;
