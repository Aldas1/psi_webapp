import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";

import { produce } from "immer";

import {
  Button,
  Card,
  CardContent,
  FormControlLabel,
  Radio,
  RadioGroup,
  Typography,
  List,
  ListItem,
} from "@mui/material";

import { PropTypes } from "prop-types";

export default function SoloGame() {
  const location = useLocation();
  const [quizId, setQuizId] = useState("");
  const [questions, setQuestions] = useState([]);
  const [answers, setAnswers] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    if (!location.state) {
      setQuizId(false);
      return;
    }
    fetch(`/api/quizzes/${location.state.quizId}/questions`)
      .then((res) => res.json())
      .then((data) => {
        setQuizId(location.state.quizId);
        setQuestions(data);
        setAnswers(
          data.map(() => {
            return {};
          })
        );
        setCurrentQuestionIndex(0);
        setLoading(false);
      });
  }, [location]);

  if (!quizId) {
    return "Whoops! Please select quiz from a list.";
  }

  if (loading) {
    return "Loading...";
  }

  let questionElement = null;
  switch (questions[currentQuestionIndex].questionType) {
    case "singleChoiceQuestion":
      questionElement = (
        <SingleChoiceQuestion
          question={questions[currentQuestionIndex]}
          answer={answers[currentQuestionIndex]}
          onAnswerChange={(newAnswer) => {
            setAnswers(
              produce(answers, (draft) => {
                draft[currentQuestionIndex] = newAnswer;
              })
            );
          }}
        />
      );
      break;
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
          <Button onClick={submit}>Submit</Button>
        ) : (
          <Button
            onClick={() => setCurrentQuestionIndex(currentQuestionIndex + 1)}
          >
            Next
          </Button>
        )}
        {questionElement}
      </CardContent>
    </Card>
  );

  function submit() {
    console.table(answers);
  }
}

function SingleChoiceQuestion({ question, answer, onAnswerChange }) {
  const options = question.questionParameters.options;

  useEffect(() => {
    if (answer.optionIndex == null) {
      onAnswerChange({ answered: false, optionIndex: -1 });
    }
  }, [answer, onAnswerChange]);

  // Material UI determines that radio group is uncontrolled on first render (before answer.optionIndex is not null)
  // this helps avoid that
  if (answer.optionIndex == null) {
    return "";
  }

  return (
    <List>
      <RadioGroup
        value={answer.optionIndex}
        onChange={(e) => {
          onAnswerChange({
            answered: true,
            optionIndex: parseInt(e.target.value),
          });
        }}
      >
        {question.questionParameters.options.map((opt, index) => {
          return (
            <ListItem key={index}>
              <FormControlLabel value={index} control={<Radio />} />
              {opt}
            </ListItem>
          );
        })}
      </RadioGroup>
    </List>
  );
}
SingleChoiceQuestion.propTypes = {
  question: PropTypes.object,
  answer: PropTypes.object,
  onAnswerChange: PropTypes.func,
};
