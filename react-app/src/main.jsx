import React from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Test from "./routes/Test";
import QuizCreator from "./routes/QuizCreator";
import { CssBaseline } from "@mui/material";
import "@fontsource/roboto";
import "./main.css";
import MainLayout from "./layouts/mainLayout";

import Root from "./routes/Root";

const router = createBrowserRouter([
  {
    element: (
      <MainLayout>
        <Root />
      </MainLayout>
    ),
    path: "/",
  },
  {
    Component: Test,
    path: "/test",
  },
  {
    element: (
      <MainLayout>
        <QuizCreator />
      </MainLayout>
    ),
    path: "/createQuiz",
  },
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <CssBaseline />
    <RouterProvider router={router} />
  </React.StrictMode>
);
