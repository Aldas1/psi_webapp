import axios from "axios";
import { UserLeaderboardResponseDto } from "../types/quiz";

async function getUsersLeaderboard(): Promise<UserLeaderboardResponseDto[]> {
  const response = await axios.get("/api/leaderboard/users");
  return response.data;
}

export { getUsersLeaderboard };
