import {
  AppBar,
  Typography,
  Toolbar,
  IconButton,
  Container,
  Paper,
} from "@mui/material";
import { PropTypes } from "prop-types";
import { Link as RouterLink } from "react-router-dom";

export default function MainLayout({ children }) {
  return (
    <>
      <Container maxWidth="lg">
        <Paper
          elevation={3}
          style={{ padding: "20px" }}
          sx={{ minHeight: "100vh" }}
        >
          <Typography
            variant="h4"
            gutterBottom
            component={RouterLink}
            to="/"
            style={{
              textDecoration: "none",
              color: "inherit",
              marginBottom: "2rem",
              display: "block",
            }}
          >
            Quiz App
          </Typography>
          <main>{children}</main>
        </Paper>
      </Container>
    </>
  );
}
MainLayout.propTypes = {
  children: PropTypes.element,
};
