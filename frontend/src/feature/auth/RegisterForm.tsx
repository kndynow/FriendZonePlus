import FormField from "../../components/ui/FormField";
import { Form } from "react-router-dom";
import { Button } from "react-bootstrap";
import { useRegisterForm } from "./useRegisterForm";

interface RegisterFormProps {
  onSwitchToLogin: () => void;
}

export default function RegisterForm({ onSwitchToLogin }: RegisterFormProps) {
  const {
    registerForm,
    errors,
    touched,
    submitting,
    setProperty,
    submitRegister,
  } = useRegisterForm();

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

  return (
    <>
      <div className="semi-transparent-bg p-3 mb-2 f-border">
        <Form onSubmit={submitRegister}>
          {fields.map((field) => (
            <FormField
              key={field.name}
              label={field.label}
              name={field.name}
              type={field.type}
              placeholder={field.placeholder}
              value={registerForm[field.name]}
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
