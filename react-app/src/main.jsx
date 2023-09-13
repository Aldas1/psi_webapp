import React from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Root from "./routes/Root";
import QuizCreator from "./routes/QuizCreator";

const router = createBrowserRouter([
  {
    Component: Root,
    path: "/",
  },
  {
    Component: QuizCreator,
    path: "/quizCreator",
  },
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
