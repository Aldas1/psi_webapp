import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";

import { Button, Card, CardContent, Typography } from "@mui/material";

export default function SoloGame() {
  const location = useLocation();
  const [quizId, setQuizId] = useState("");
  const [questions, setQuestions] = useState([]);
  const [answers, setAnswers] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  useEffect(() => {
    if (!location.state) {
      setQuizId(false);
      return;
    }
    setQuizId(location.state.quizId);
    fetch(`/api/quizzes/${location.state.quizId}/questions`)
      .then((res) => res.json())
      .then((data) => {
        setQuestions(data);
        setAnswers(
          data.map(() => {
            return {};
          })
        );
        setCurrentQuestionIndex(0);
      });
  }, [location]);

  if (!quizId) {
    return "Whoops! Please select quiz from a list.";
  }

  return (
    <Card>
      <CardContent sx={{ minHeight: "25rem" }}>
        <Typography variant="h6">
          Question #{currentQuestionIndex + 1} of {questions.length}
        </Typography>
        <Typography variant="h5" sx={{ fontWeight: "bold" }}>
          {questions[currentQuestionIndex].questionText}
        </Typography>
        <Button
          disabled={currentQuestionIndex === 0}
          onClick={() => setCurrentQuestionIndex(currentQuestionIndex - 1)}
        >
          Previous
        </Button>
        {currentQuestionIndex === questions.length - 1 ? (
          <Button>Submit</Button>
        ) : (
          <Button
            onClick={() => setCurrentQuestionIndex(currentQuestionIndex + 1)}
          >
            Next
          </Button>
        )}
      </CardContent>
    </Card>
  );
}
