import { Button, Heading, Text, VStack } from "@chakra-ui/react";
import { Link } from "react-router-dom";

function NotFound() {
  return (
    <>
      <VStack>
        <Heading>Page Not Found</Heading>
        <Text fontSize="xl">It seems you are lost 😢</Text>
        <Link to="/">
          <Button>Go Home</Button>
        </Link>
      </VStack>
    </>
  );
}

export default NotFound;
