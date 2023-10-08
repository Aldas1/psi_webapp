import { useLoaderData, Link as RouterLink } from "react-router-dom";

import { Fragment } from "react";

import { Divider, List, ListItem, Typography, Button } from "@mui/material";

export default function QuizList() {
  const quizzes = useLoaderData();

  return (
    <>
      <Typography variant="h5">Quiz List</Typography>
      <List>
        {quizzes.map((q) => (
          <Fragment key={q.id}>
            <Divider />
            <ListItem>
              {q.name}
              <Button
                component={RouterLink}
                to="/soloGame"
                state={{ quizId: q.id }}
              >
                Solo Play
              </Button>
              <Button
                onClick={() => {
                  fetch(`/api/quizzes/${q.id}`, { method: "DELETE" }).then(window.location.reload());
                }}
                variant="contained"
                color="error"
              >
                Delete
              </Button>
              <Divider />
            </ListItem>
            <Divider />
          </Fragment>
        ))}
      </List>
    </>
  );
}
