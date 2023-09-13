import { Link } from "react-router-dom";
import "./Root.css";

export default function Root() {
  return (
    <nav>
      <Link to="/">Home</Link>
      <Link to="/quizCreator">QuizCreator</Link>
    </nav>
  );
}
