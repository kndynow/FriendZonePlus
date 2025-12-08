import { useState } from "react";
import { Button } from "react-bootstrap";
import { useAuth } from "../../../context/AuthProvider";
import { Form, useNavigate } from "react-router-dom";
import FormField from "../../components/ui/FormField";
import toast from "react-hot-toast";

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

export default function LoginForm({ onSwitchToRegister }: LoginFormProps) {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [form, setForm] = useState({ usernameOrEmail: "", password: "" });
  const [submitting, setSubmitting] = useState(false);

  const fields = [
    {
      name: "usernameOrEmail",
      label: "Username or Email",
      type: "text",
      placeholder: "Username or email",
    },
    {
      name: "password",
      label: "Password",
      type: "password",
      placeholder: "Enter password",
    },
  ] as const;

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      setSubmitting(true);
      await login({
        usernameOrEmail: form.usernameOrEmail,
        password: form.password,
      });
      toast.success("Log in successful!");
      navigate("/");
    } catch (err: any) {
      const message = (err && (err.message || err.detail)) || "Login failed";
      toast.error(message);
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <>
      <div className="semi-transparent-bg p-3 f-border">
        <Form onSubmit={handleSubmit}>
          {fields.map((field) => (
            <FormField
              key={field.name}
              label={field.label}
              name={field.name}
              type={field.type}
              placeholder={field.placeholder}
              value={form[field.name]}
              onChange={setProperty}
            />
          ))}

          <Button
            className="mt-2 w-100"
            variant="primary"
            disabled={submitting}
            type="submit"
          >
            {submitting ? "Logging in..." : "Log in"}
          </Button>
        </Form>

        <p className="text-center mt-3">
          No account yet?
          <span className="text-primary px-2" onClick={onSwitchToRegister}>
            Register here!
          </span>
        </p>
      </div>
    </>
  );
}
