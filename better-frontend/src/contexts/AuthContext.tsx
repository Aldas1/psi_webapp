import {
  createContext,
  Dispatch,
  ReactNode,
  SetStateAction,
  useEffect,
  useState,
} from "react";

import { setToken } from "../api/quizzes";

interface AuthInfo {
  username: string;
  token: string;
}

const AuthContext = createContext<
  [AuthInfo | null, Dispatch<SetStateAction<AuthInfo | null>>]
>([null, () => {}]);

function AuthProvider({ children }: { children: ReactNode }) {
  const [authInfo, setAuthInfo] = useState<AuthInfo | null>(
    JSON.parse(localStorage.getItem("authInfo") ?? "null")
  );

  useEffect(() => {
    if (authInfo === null) {
      localStorage.removeItem("authInfo");
    } else {
      localStorage.setItem("authInfo", JSON.stringify(authInfo));
    }
    setToken(authInfo?.token);
  }, [authInfo]);
  return (
    <AuthContext.Provider value={[authInfo, setAuthInfo]}>
      {children}
    </AuthContext.Provider>
  );
}

export { AuthProvider, AuthContext };
