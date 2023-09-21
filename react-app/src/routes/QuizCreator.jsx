import {
  Button,
  List,
  ListItem,
  TextField,
  Typography,
  Divider,
  IconButton,
  Box,
  MenuItem,
  RadioGroup,
  Radio,
  FormControlLabel,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import AddIcon from "@mui/icons-material/Add";
import { Fragment, useState } from "react";
import { Question } from "../data/Question";
import { PropTypes } from "prop-types";
import { produce } from "immer";
import uniqid from "uniqid";

export default function QuizCreator() {
  const [quizName, setQuizName] = useState("");
  const [questions, setQuestions] = useState([]);
  const [dataNotProvided, setDataNotProvided] = useState(false);
  return (
    <div>
      <Typography variant="h5" gutterBottom>
        Quiz Creation
      </Typography>

      <TextField
        label="Quiz Name"
        required
        inputProps={{ maxLength: 100 }}
        fullWidth
        value={quizName}
        onChange={(e) => setQuizName(e.target.value)}
        margin="normal"
      ></TextField>

      <Typography variant="h6" gutterBottom>
        Questions
      </Typography>

      <List>
        {questions.map((q, index) => {
          return (
            <Fragment key={q.id}>
              <Divider />
              <ListItem>
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    width: "100%",
                  }}
                >
                  <QuestionCreator
                    question={q}
                    onQuestionChange={(changedQ) => {
                      const newQuestions = [...questions];
                      newQuestions[index] = changedQ;
                      setQuestions(
                        produce(questions, (draft) => {
                          draft[index] = changedQ;
                        })
                      );
                    }}
                  />
                  <IconButton
                    onClick={() => {
                      setQuestions(questions.filter((_, i) => i !== index));
                    }}
                    sx={{ alignSelf: "flex-start" }}
                  >
                    <DeleteIcon />
                  </IconButton>
                </Box>
              </ListItem>
              <Divider />
            </Fragment>
          );
        })}
      </List>

      <Button onClick={addQuestion}>Add Question</Button>

      <Button variant="contained" onClick={submitQuiz}>
        Submit
      </Button>

      {dataNotProvided && (
        <p style={{ display: "inline", color: "red" }}>
          Some fields are not filled
        </p>
      )}
    </div>
  );

  function addQuestion() {
    setQuestions([...questions, Question()]);
  }

  function submitQuiz() {
    if (quizName.length === 0) {
      setDataNotProvided(true);
      return;
    }
    console.log("Submitting quiz");
    const payload = { name: quizName, questions: [] };
    for (const q of questions) {
      if (q.questionText.length === 0) {
        setDataNotProvided(true);
        return;
      }
      const questionData = {
        questionText: q.questionText,
        questionType: q.questionType,
        questionsParameters: {},
      };
      switch (questionData.questionType) {
        case "singleChoiceQuestion":
          questionData.questionsParameters.correctOptionIndex =
            q.parameters.correctOptionIndex;
          questionData.questionsParameters.options = q.parameters.options.map(
            (opt) => opt.value
          );
          break;
        default:
          throw "Not implemented";
      }
      payload.questions.push(questionData);
    }
    setDataNotProvided(false);
    console.table(payload);
  }
}

function QuestionCreator({ question, onQuestionChange }) {
  let questionBodyCreator = null;
  switch (question.questionType) {
    case "singleChoiceQuestion":
      questionBodyCreator = (
        <SingleChoiceQuestionCreator
          question={question}
          onQuestionChange={onQuestionChange}
        />
      );
      break;
  }

  return (
    <div>
      <TextField
        label="Question name"
        value={question.questionText || ""}
        onChange={(e) =>
          onQuestionChange({
            ...question,
            questionText: e.target.value,
          })
        }
        margin="normal"
        inputProps={{ maxLength: 100 }}
        required
      ></TextField>
      <TextField
        select
        value={question.questionType}
        label="Question Type"
        sx={{ width: "20rem" }}
        margin="normal"
        onChange={(e) => {
          console.log("here");
          onQuestionChange(Question(question.questionText, e.target.value));
        }}
      >
        <MenuItem value="singleChoiceQuestion">Single Choice Question</MenuItem>
      </TextField>
      {questionBodyCreator}
    </div>
  );
}
QuestionCreator.propTypes = {
  question: PropTypes.object,
  onQuestionChange: PropTypes.func,
};

function SingleChoiceQuestionCreator({ question, onQuestionChange }) {
  return (
    <div>
      <Typography variant="h6">Options</Typography>
      <List>
        <RadioGroup
          value={question.parameters.correctOptionIndex}
          onChange={(e) => {
            onQuestionChange(
              produce(question, (draft) => {
                draft.parameters.correctOptionIndex = parseInt(e.target.value);
              })
            );
          }}
        >
          {question.parameters.options.map((opt, index) => {
            return (
              <ListItem key={opt.id}>
                <TextField
                  value={opt.value}
                  onChange={(e) =>
                    onQuestionChange(
                      produce(question, (draft) => {
                        draft.parameters.options[index].value = e.target.value;
                      })
                    )
                  }
                ></TextField>
                <FormControlLabel control={<Radio />} value={index} />
                <IconButton
                  onClick={() =>
                    onQuestionChange(
                      produce(question, (draft) => {
                        draft.parameters.correctOptionIndex = 0;
                        draft.parameters.options =
                          draft.parameters.options.filter(
                            (o) => o.id !== opt.id
                          );
                      })
                    )
                  }
                >
                  <DeleteIcon />
                </IconButton>
              </ListItem>
            );
          })}
        </RadioGroup>
      </List>
      <IconButton
        onClick={() =>
          onQuestionChange(
            produce(question, (draft) => {
              draft.parameters.options.push({ id: uniqid(), value: "" });
            })
          )
        }
      >
        <AddIcon />
      </IconButton>
    </div>
  );
}
SingleChoiceQuestionCreator.propTypes = {
  question: PropTypes.object,
  onQuestionChange: PropTypes.func,
};
