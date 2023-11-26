import { Route, Routes } from "react-router-dom";
import NotFound from "./pages/NotFound";
import Home from "./pages/Home";
import MainLayout from "./layouts/MainLayout";
import CreateQuiz from "./pages/CreateQuiz";
import QuizPreview from "./pages/QuizPreview";
import Auth from "./pages/Auth";
import ContainerLayout from "./layouts/ContainerLayout";
import UserLeaderboard from "./pages/UserLeaderboard";
import FlashardCollections from "./pages/FlashcardCollections";

function Router() {
  return (
    <Routes>
      <Route element={<MainLayout />}>
        <Route element={<ContainerLayout />}>
          <Route path="/" element={<Home />} />
          <Route path="/quizzes/:id" element={<QuizPreview />} />
          <Route path="/createQuiz" element={<CreateQuiz />} />
          <Route path="/auth" element={<Auth />} />
          <Route path="/leaderboard/users" element={<UserLeaderboard />} />
          <Route
            path="/flashcard-collections"
            element={<FlashardCollections />}
          />
          <Route path="*" element={<NotFound />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default Router;
