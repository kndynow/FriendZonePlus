import { Button } from "react-bootstrap";
import { useLoginForm } from "./useLoginForm";
import FormField from "../../components/ui/FormField";

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

export default function LoginForm({ onSwitchToRegister }: LoginFormProps) {
  const { loginForm, submitting, setProperty, submitLogin } = useLoginForm();

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

  return (
    <>
      <div className="semi-transparent-bg p-3 f-border">
        <form onSubmit={submitLogin}>
          {fields.map((field) => (
            <FormField
              key={field.name}
              label={field.label}
              name={field.name}
              type={field.type}
              placeholder={field.placeholder}
              value={loginForm[field.name]}
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
        </form>

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
