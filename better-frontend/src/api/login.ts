import axios from "axios";

async function login(username: string, password: string): Promise<string> {
  const response = await axios.post("/api/login", { username, password });
  return response.data.token;
}

export { login };
