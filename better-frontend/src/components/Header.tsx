import { Flex, IconButton, useColorMode, Heading } from "@chakra-ui/react";
import { MoonIcon, SunIcon } from "@chakra-ui/icons";
import { Link as ReactRouterLink } from "react-router-dom";

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
  return (
    <Flex
      minHeight="4.5rem"
      alignItems="center"
      position="sticky"
      top={0}
      justifyContent="space-between"
    >
      <ReactRouterLink to="/">
        <Heading>Quiz App</Heading>
      </ReactRouterLink>
      <DarkModeToggle />
    </Flex>
  );
}

export default Header;