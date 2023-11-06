import {
  Button,
  FormControl,
  FormErrorMessage,
  FormLabel,
  HStack,
  Heading,
  Input,
  List,
  ListItem,
  VStack,
} from "@chakra-ui/react";
import { AuthContext } from "../contexts/AuthContext";
import { useContext, useEffect, useState } from "react";
import { login as sendLogin } from "../api/login";
import { useNavigate } from "react-router-dom";
import { Link as ChakraLink } from "@chakra-ui/react";
import { register as sendRegister } from "../api/users";
import { AxiosError } from "axios";

function Login({ toggleMode }: { toggleMode: () => void }) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [authInfo, setAuthInfo] = useContext(AuthContext);

  async function login() {
    if (submitting) {
      return;
    }
    const gotUsername = username;
    setSubmitting(true);
    setSubmitError(null);
    try {
      const token = await sendLogin(gotUsername, password);
      setAuthInfo({ username: gotUsername, token });
    } catch (err) {
      setSubmitError("Failed to login");
    }
    setSubmitting(false);
  }

  return (
    <FormControl isInvalid={submitError !== null}>
      <VStack>
        <Heading>Log In</Heading>
        <FormControl>
          <FormLabel>Username</FormLabel>
          <Input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </FormControl>
        <FormControl>
          <FormLabel>Password</FormLabel>
          <Input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </FormControl>
        <HStack>
          <Button onClick={login}>Log In</Button>
          <ChakraLink onClick={() => toggleMode()}>or register</ChakraLink>
          <FormErrorMessage>{submitError}</FormErrorMessage>
        </HStack>
      </VStack>
    </FormControl>
  );
}

function Register({ toggleMode }: { toggleMode: () => void }) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [errors, setErrors] = useState<string[]>([]);
  const [authInfo, setAuthInfo] = useContext(AuthContext);

  async function register() {
    if (submitting) {
      return;
    }
    const gotUsername = username;
    const gotPassword = password;
    if (gotPassword !== confirmPassword) {
      setErrors(["Passwords do not match"]);
      setSubmitting(false);
      return;
    }
    setSubmitting(true);
    setErrors([]);
    try {
      await sendRegister(gotUsername, gotPassword);
      const token = await sendLogin(gotUsername, gotPassword);
      setAuthInfo({ username: gotUsername, token });
    } catch (err: unknown) {
      if (err instanceof AxiosError) {
        if (err.response && err.response.status === 409) {
          setErrors(["User already exists"]);
          setSubmitting(false);
          return;
        }
        const errors = err.response?.data.errors;
        setErrors(Object.keys(errors).map((k) => errors[k]));
        setSubmitting(false);
        return;
      }
    }
    setSubmitting(false);
  }

  return (
    <FormControl isInvalid={errors.length > 0}>
      <VStack>
        <Heading>Register</Heading>
        <FormControl>
          <FormLabel>Username</FormLabel>
          <Input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </FormControl>
        <FormControl>
          <FormLabel>Password</FormLabel>
          <Input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </FormControl>
        <FormControl>
          <FormLabel>Confirm password</FormLabel>
          <Input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
        </FormControl>
        <HStack>
          <Button onClick={register}>Register</Button>
          <ChakraLink onClick={() => toggleMode()}>or login</ChakraLink>
        </HStack>
        <FormErrorMessage>
          <List>
            {errors.map((error, i) => (
              <ListItem key={i}>{error}</ListItem>
            ))}
          </List>
        </FormErrorMessage>
      </VStack>
    </FormControl>
  );
}

function Auth() {
  const [authInfo, setAuthInfo] = useContext(AuthContext);
  const navigate = useNavigate();
  const [loginShown, setLoginShown] = useState(true);

  useEffect(() => {
    if (authInfo) {
      navigate("/");
    }
  }, [authInfo]);

  if (loginShown) {
    return <Login toggleMode={() => setLoginShown(false)} />;
  }
  return <Register toggleMode={() => setLoginShown(true)} />;
}

export default Auth;
