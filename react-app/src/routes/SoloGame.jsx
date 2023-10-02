import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
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
  const [quizId, setQuizId] = useState(null);
  const [questions, setQuestions] = useState([]);
  const [answers, setAnswers] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [loading, setLoading] = useState(true);
  const [answered, setAnswered] = useState(false);
  const [score, setScore] = useState(0);
  const [correctlyAnswered, setCorrectlyAnswered] = useState(0);

  useEffect(() => {
    if (!location.state) {
      setQuizId(null);
      return;
    }
    fetch(`/api/quizzes/${location.state.quizId}/questions`)
      .then((res) => res.json())
      .then((data) => {
        setQuizId(location.state.quizId);
        setQuestions(data);
        setAnswers([]);
        setCurrentQuestionIndex(0);
        setLoading(false);
      });
  }, [location]);

  if (answered) {
    return `Score: ${score}. Correctly answered ${correctlyAnswered} out of ${questions.length}`;
  }

  if (quizId == null) {
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
          answer={answers.find(
            (a) => a.questionId === questions[currentQuestionIndex].id
          )}
          onAnswerChange={(newAnswer) => {
            setAnswers(
              produce(answers, (draft) => {
                let answerIndex = draft.findIndex(
                  (a) => a.questionId === newAnswer.questionId
                );
                if (answerIndex === -1) {
                  draft.push(newAnswer);
                } else {
                  draft[answerIndex] = newAnswer;
                }
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

  async function submit() {
    const response = await axios.post(`/api/quizzes/${quizId}/submit`, answers);
    if (response.data.status === "success") {
      setAnswered(true);
      setScore(response.data.score);
      setCorrectlyAnswered(response.data.correctlyAnswered);
    }
  }
}

function SingleChoiceQuestion({ question, answer, onAnswerChange }) {
  const options = question.questionParameters.options;

  let optionName = null;
  if (answer) {
    optionName = answer.optionName;
  }

  return (
    <List>
      <RadioGroup
        value={optionName}
        onChange={(e) => {
          onAnswerChange({
            questionId: question.id,
            optionName: e.target.value,
          });
        }}
      >
        {options.map((opt, index) => {
          return (
            <ListItem key={index}>
              <FormControlLabel value={opt} control={<Radio />} />
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
