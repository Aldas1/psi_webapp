import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const connectionUrl = "https://localhost:7280/discussionHub";

class DiscussionHub {
  connection: HubConnection;

  constructor() {
    this.connection = new HubConnectionBuilder().withUrl(connectionUrl).build();
  }

  async openConnection() {
    this.connection = new HubConnectionBuilder().withUrl(connectionUrl).build();
    await this.connection.start();
  }
}

export default DiscussionHub;
