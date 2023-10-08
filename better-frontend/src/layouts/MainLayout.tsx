import { Outlet } from "react-router-dom";
import Header from "../components/Header";
import { Box } from "@chakra-ui/layout";

function MainLayout() {
  return (
    <Box marginX="1rem">
      <Header />
      <Box marginY="2rem">
        <Outlet />
      </Box>
    </Box>
  );
}

export default MainLayout;
