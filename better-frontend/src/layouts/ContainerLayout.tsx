import { Container } from "@chakra-ui/react";
import { Outlet } from "react-router-dom";

function ContainerLayout() {
  return (
    <Container maxWidth="3xl">
      <Outlet />
    </Container>
  );
}

export default ContainerLayout;
