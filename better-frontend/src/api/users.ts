import axios from "axios";

async function register(username: string, password: string) {
  await axios.post("/api/users", { username, password });
}

export { register };
