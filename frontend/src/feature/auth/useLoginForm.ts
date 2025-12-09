import { useState } from "react";
import { useAuth } from "../../../context/AuthProvider";
import toast from "react-hot-toast";

export function useLoginForm() {
  const { login } = useAuth();

  const [loginForm, setLoginForm] = useState({
    usernameOrEmail: "",
    password: "",
  });

  const [submitting, setSubmitting] = useState(false);

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;
    setLoginForm((prev) => ({ ...prev, [name]: value }));
  }

  async function submitLogin(e?: React.FormEvent): Promise<boolean> {
    if (e) e.preventDefault();

    try {
      setSubmitting(true);
      await login(loginForm);
      return true;
    } catch (err: any) {
      toast.error(err?.message || err?.detail || "Login failed");
      return false;
    } finally {
      setSubmitting(false);
    }
  }

  return {
    loginForm,
    submitting,
    setProperty,
    submitLogin,
  };
}
