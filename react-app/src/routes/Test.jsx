import { Link } from "react-router-dom";
import { useEffect, useState } from "react";

export default function Root() {
  const [authors, setAuthors] = useState(null);
  useEffect(() => {
    let ignore = false;
    fetch("/api/authors")
      .then((res) => res.json())
      .then((res) => {
        if (!ignore) {
          setAuthors(res);
        }
      })
      .catch(() => setAuthors(["ERROR"]));
    return () => (ignore = true);
  }, []);
  return (
    <>
      <nav>
        <Link to="/">Home</Link>
        <Link to="/quizCreator">QuizCreator</Link>
      </nav>
      {authors && <p>Authors: {authors.join(", ")}</p>}
    </>
  );
}
