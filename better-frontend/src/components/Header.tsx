import {
  Flex,
  IconButton,
  useColorMode,
  Heading,
  HStack,
  Button,
} from "@chakra-ui/react";
import { MoonIcon, SunIcon } from "@chakra-ui/icons";
import { Link as ReactRouterLink } from "react-router-dom";
import { useTheme } from "@chakra-ui/react";

function DarkModeToggle() {
  const { colorMode, toggleColorMode } = useColorMode();

  return (
    <IconButton
      aria-label="Toggle dark mode"
      onClick={toggleColorMode}
      icon={colorMode === "light" ? <MoonIcon /> : <SunIcon />}
    />
  );
}

function Header() {
  const theme = useTheme();
  return (
    <Flex
      minHeight="4.5rem"
      alignItems="center"
      position="sticky"
      top={0}
      justifyContent="space-between"
      zIndex="1000"
      bg={theme.styles.global.body.bg}
    >
      <ReactRouterLink to="/">
        <Heading>Quiz App</Heading>
      </ReactRouterLink>
      <HStack>
        <DarkModeToggle />
        <ReactRouterLink to="/createQuiz">
          <Button>Create Quiz</Button>
        </ReactRouterLink>
      </HStack>
    </Flex>
  );
}

export default Header;
