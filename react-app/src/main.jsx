import React from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Test from "./routes/Test";
import QuizCreator from "./routes/QuizCreator";
import QuizList from "./routes/QuizList";
import { CssBaseline } from "@mui/material";
import "@fontsource/roboto";
import "./main.css";
import MainLayout from "./layouts/mainLayout";

import Root from "./routes/Root";
import SoloGame from "./routes/SoloGame";

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
  {
    element: (
      <MainLayout>
        <QuizList />
      </MainLayout>
    ),
    path: "/quizList",
    async loader() {
      const response = await fetch("/api/quizzes").then();
      const data = await response.json();
      return data;
    },
  },
  {
    element: (
      <MainLayout>
        <SoloGame />
      </MainLayout>
    ),
    path: "/soloGame",
  },
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <CssBaseline />
    <RouterProvider router={router} />
  </React.StrictMode>
);
