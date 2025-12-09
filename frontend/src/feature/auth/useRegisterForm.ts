import { useState } from "react";
import { useAuth } from "../../../context/AuthProvider";
import { validateRegister } from "../../../utils/validation";
import toast from "react-hot-toast";
import type { RegisterRequest } from "../../../types/auth";

export function useRegisterForm() {
  const { register } = useAuth();

  const [registerForm, setregisterForm] = useState<RegisterRequest>({
    username: "",
    email: "",
    password: "",
    firstName: "",
    lastName: "",
  });

  const [touched, setTouched] = useState<Record<string, boolean>>({});
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [submitting, setSubmitting] = useState(false);

  function markTouched(field: string) {
    setTouched((t) => ({ ...t, [field]: true }));
  }

  function markAllTouched() {
    const allTouched = Object.fromEntries(
      Object.keys(registerForm).map((key) => [key, true])
    );
    setTouched(allTouched);
  }

  function validateForm(updatedForm: RegisterRequest) {
    const validation = validateRegister(updatedForm);
    setErrors(validation);
    return validation;
  }

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    const updated = { ...registerForm, [name]: value };

    setregisterForm(updated);
    markTouched(name);
    validateForm(updated);
  }

  async function submitRegister(e?: React.FormEvent) {
    if (e) e.preventDefault();

    markAllTouched();

    const validation = validateForm(registerForm);
    if (Object.keys(validation).length > 0) return false;

    try {
      setSubmitting(true);
      await register(registerForm);
      toast.success("Account created successfully!");
      return true;
    } catch (err: any) {
      toast.error(
        err?.errors?.username?.[0] ||
          err?.errors?.email?.[0] ||
          "Unable to register account."
      );
      return false;
    } finally {
      setSubmitting(false);
    }
  }

  return {
    registerForm,
    errors,
    touched,
    submitting,
    setProperty,
    submitRegister,
  };
}
