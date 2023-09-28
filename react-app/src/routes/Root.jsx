import { Grid, Card, CardContent, Typography, Button } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";

export default function Root() {
  return (
    <Grid container spacing={2} justifyContent="center">
      {/* Create Quiz Button */}
      <Grid item xs={12} sm={6}>
        <Card>
          <CardContent>
            <Typography variant="h5" component="div">
              Create Quiz
            </Typography>
            <Button
              component={RouterLink}
              to="/createQuiz"
              variant="contained"
              color="primary"
            >
              Go
            </Button>
          </CardContent>
        </Card>
      </Grid>

      {/* List Quizzes Button */}
      <Grid item xs={12} sm={6}>
        <Card>
          <CardContent>
            <Typography variant="h5" component="div">
              List Quizzes
            </Typography>
            <Button
              component={RouterLink}
              to="/quizList"
              variant="contained"
              color="primary"
            >
              Go
            </Button>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
}
