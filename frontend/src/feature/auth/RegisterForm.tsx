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

  const fields = [
    {
      name: "firstName",
      label: "First name",
      type: "text",
      placeholder: "Enter first name",
    },
    {
      name: "lastName",
      label: "Last name",
      type: "text",
      placeholder: "Enter last name",
    },
    {
      name: "email",
      label: "Email",
      type: "email",
      placeholder: "Enter email",
    },
    {
      name: "username",
      label: "Username",
      type: "text",
      placeholder: "Enter username",
    },
    {
      name: "password",
      label: "Password",
      type: "password",
      placeholder: "Password",
    },
  ] as const;

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    const updatedForm = { ...form, [name]: value };
    setForm(updatedForm);

    setTouched((prev) => ({ ...prev, [name]: true }));
    setErrors(validateRegister(updatedForm));
  }

  async function handleRegister(event: React.FormEvent) {
    event.preventDefault();

    const allTouched = Object.fromEntries(
      Object.keys(form).map((k) => [k, true])
    ) as Record<keyof RegisterRequest, boolean>;

    setTouched(allTouched);

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
      const userErr = err?.errors?.username?.[0];
      const emailErr = err?.errors?.email?.[0];

      toast.error(userErr || emailErr || "Unable to register account.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <>
      <div className="semi-transparent-bg p-3 mb-2 f-border">
        <Form onSubmit={handleRegister}>
          {fields.map((field) => (
            <FormField
              key={field.name}
              label={field.label}
              name={field.name}
              type={field.type}
              placeholder={field.placeholder}
              value={form[field.name]}
              onChange={setProperty}
              error={errors[field.name]}
              touched={touched[field.name]}
            />
          ))}

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
      </div>
    </>
  );
}
