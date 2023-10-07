import { Outlet } from "react-router-dom";
import Header from "../components/Header";
import { Box } from "@chakra-ui/layout";

function MainLayout() {
  return (
    <Box marginX="1rem">
      <Header />
      <Box marginTop="1rem">
        <Outlet />
      </Box>
    </Box>
  );
}

export default MainLayout;
