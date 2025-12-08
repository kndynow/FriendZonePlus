import toast from "react-hot-toast";
import { validateRegister } from "../../../utils/validation";
import type { RegisterRequest } from "../../../types/auth";
import { useState } from "react";
import { useAuth } from "../../../context/AuthProvider";
import FormField from "../../components/ui/FormField";
import { Form } from "react-router-dom";
import { Button } from "react-bootstrap";

interface RegisterFormProps {
  onSwitchToLogin: () => void;
}

export default function RegisterForm({ onSwitchToLogin }: RegisterFormProps) {
  const [form, setForm] = useState<RegisterRequest>({
    username: "",
    email: "",
    password: "",
    firstName: "",
    lastName: "",
  });

  const { register } = useAuth();
  const [submitting, setSubmitting] = useState(false);

  const [touched, setTouched] = useState<
    Partial<Record<keyof RegisterRequest, boolean>>
  >({});

  const [errors, setErrors] = useState<
    Partial<Record<keyof RegisterRequest, string>>
  >({});

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    const updatedForm = { ...form, [name]: value };
    setForm(updatedForm);

    setTouched((prev) => ({ ...prev, [name]: true }));
    setErrors(validateRegister(updatedForm));
  }

  async function handleRegister(event: React.FormEvent) {
    event.preventDefault();

    setTouched({
      username: true,
      email: true,
      password: true,
      firstName: true,
      lastName: true,
    });

    const frontendErrors = validateRegister(form);
    setErrors(frontendErrors);
    if (Object.keys(frontendErrors).length > 0) {
      return; // Stop submission if there are frontend errors
    }

    try {
      setSubmitting(true);
      await register(form);

      toast.success("Account created successfully!");
    } catch (err: any) {
      if (err?.errors?.username) {
        toast.error(err.errors.username[0], { id: "register-error" });
        return;
      }

      if (err?.errors?.email) {
        toast.error(err.errors.email[0], { id: "register-error" });
        return;
      }

      toast.error("Unable to register account.", { id: "register-error" });
    } finally {
      setSubmitting(false);
    }
  }
  return (
    <>
      <Form onSubmit={handleRegister}>
        <FormField
          label="First name"
          name="firstName"
          placeholder="Enter first name"
          value={form.firstName}
          onChange={setProperty}
          error={errors.firstName}
          touched={touched.firstName}
        />

        <FormField
          label="Last name"
          name="lastName"
          placeholder="Enter last name"
          value={form.lastName}
          onChange={setProperty}
          error={errors.lastName}
          touched={touched.lastName}
        />

        <FormField
          label="Email"
          type="email"
          name="email"
          placeholder="Enter email"
          value={form.email}
          onChange={setProperty}
          error={errors.email}
          touched={touched.email}
        />

        <FormField
          label="Username"
          name="username"
          placeholder="Enter username"
          value={form.username}
          onChange={setProperty}
          error={errors.username}
          touched={touched.username}
        />

        <FormField
          label="Password"
          type="password"
          name="password"
          placeholder="Password"
          value={form.password}
          onChange={setProperty}
          error={errors.password}
          touched={touched.password}
        />

        <Button
          className="mt-2 w-100"
          variant="primary"
          disabled={submitting || Object.keys(errors).length > 0}
          type="submit"
        >
          {submitting ? "Registering..." : "Register"}
        </Button>
      </Form>
      <p className="text-center mt-3">
        Already have an account?
        <span className="text-primary px-2" onClick={onSwitchToLogin}>
          Sign in here!
        </span>
      </p>
    </>
  );
}
