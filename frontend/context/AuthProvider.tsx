import {
  type ReactNode,
  createContext,
  useContext,
  useState,
  useEffect,
} from "react";
import type {
  RegisterRequest,
  User,
  LoginRequest,
  AuthResponse,
} from "../types/auth.ts";

interface AuthContextType {
  user: User | null;
  register: (data: RegisterRequest) => Promise<any>;
  login: (data: LoginRequest) => Promise<any>;
  logout: () => Promise<void>;
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

const USER_KEY = "currentUser";

//Provides authentication context to all children
export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  // Fetches the user from localstorage
  useEffect(() => {
    const storedUser = localStorage.getItem(USER_KEY);
    if (storedUser) {
      try {
        setUser(JSON.parse(storedUser));
      } catch {
        localStorage.removeItem(USER_KEY);
      }
    }
    setLoading(false);
  }, []);

  const persistUser = (u: User | null) => {
    if (u) {
      localStorage.setItem(USER_KEY, JSON.stringify(u));
      setUser(u);
    } else {
      localStorage.removeItem(USER_KEY);
      setUser(null);
    }
  };

  const login = async (data: LoginRequest) => {
    try {
      setLoading(true);

      const response = await fetch("api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
      });

      const result = await response.json();

      if (!response.ok) {
        throw result;
      } else {
        if (result?.userId && result?.username) {
          persistUser({
            id: result.userId,
            username: result.username,
            email: result.email ?? "",
            firstName: result.firstName ?? "",
            lastName: result.lastName ?? "",
          });
        }
      }
      return result as AuthResponse;
    } finally {
      setLoading(false);
    }
  };

  const logout = async () => {
    try {
      await fetch("/api/auth/logout", {
        method: "POST",
        credentials: "include",
      });
    } catch {
      // ignore network errors on logout
    } finally {
      persistUser(null);
    }
  };

  const register = async (data: RegisterRequest) => {
    try {
      setLoading(true);

      const response = await fetch("/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
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
    <AuthContext.Provider value={{ user, register, login, loading }}>
      {children}
    </AuthContext.Provider>
  );
}
