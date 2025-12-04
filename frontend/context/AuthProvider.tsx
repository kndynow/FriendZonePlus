import {
  type ReactNode,
  createContext,
  useContext,
  useState,
  useEffect,
} from "react";
import type { RegisterRequest, User } from "../types/auth.ts";

interface AuthContextType {
  user: User | null;
  register: (data: RegisterRequest) => Promise<any>;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

//Provides authentication context to all children
export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  const register = async (data: RegisterRequest) => {
    try {
      setLoading(true);

      const response = await fetch("/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });

      const result = await response.json();

      if (!response.ok) {
        throw result;
      }

      return result;
    } finally {
      setLoading(false);
    }
  };

  //Return context provider
  return (
    <AuthContext.Provider value={{ user, register, loading }}>
      {children}
    </AuthContext.Provider>
  );
}
