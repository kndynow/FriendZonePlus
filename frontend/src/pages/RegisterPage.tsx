import { Button, Form, Card } from "react-bootstrap";
import { useState } from "react";
import { useAuth } from "../../context/AuthProvider";
import type { RegisterRequest } from "../../types/auth";
import { validateRegister } from "../../utils/validation";
import toast from "react-hot-toast";

export default function RegisterPage() {
  const [form, setForm] = useState<RegisterRequest>({
    username: "",
    email: "",
    password: "",
    firstName: "",
    lastName: "",
  });

  const { register } = useAuth();
  const [submitting, setSubmitting] = useState(false);

  // Track which fields have been touched
  const [touched, setTouched] = useState<
    Partial<Record<keyof RegisterRequest, boolean>>
  >({});

  // Store validation errors
  const [errors, setErrors] = useState<
    Partial<Record<keyof RegisterRequest, string>>
  >({});

  // Update form value, mark field as touched, and validate
  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    const updatedForm = { ...form, [name]: value };
    setForm(updatedForm);

    setTouched((prev) => ({ ...prev, [name]: true }));
    setErrors(validateRegister(updatedForm));
  }

  async function handleRegister(event: React.FormEvent) {
    event.preventDefault();

    // Marks all fields as touched so all errors show if submit is clicked
    setTouched({
      username: true,
      email: true,
      password: true,
      firstName: true,
      lastName: true,
    });

    // Check frontend validation before sending request
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
    <Card className="mt-5 mx-2">
      <Card.Body>
        <Card.Title className="text-center">Register</Card.Title>

        <Form onSubmit={handleRegister}>
          <Form.Group>
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              name="email"
              value={form.email}
              placeholder="Enter email"
              onChange={setProperty}
              isInvalid={!!errors.email && !!touched.email}
              required
            />
            <Form.Control.Feedback type="invalid">
              {touched.email && errors.email}
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group>
            <Form.Label>Username</Form.Label>
            <Form.Control
              type="text"
              name="username"
              value={form.username}
              placeholder="Enter username"
              onChange={setProperty}
              isInvalid={!!errors.username && !!touched.username}
              required
            />
            <Form.Control.Feedback type="invalid">
              {touched.username && errors.username}
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              name="password"
              value={form.password}
              placeholder="Password"
              onChange={setProperty}
              isInvalid={!!errors.password && !!touched.password}
              required
            />
            <Form.Control.Feedback type="invalid">
              {touched.password && errors.password}
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group>
            <Form.Label>First name</Form.Label>
            <Form.Control
              type="text"
              name="firstName"
              value={form.firstName}
              placeholder="Enter first name"
              onChange={setProperty}
              isInvalid={!!errors.firstName && !!touched.firstName}
              required
            />
            <Form.Control.Feedback type="invalid">
              {touched.firstName && errors.firstName}
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group>
            <Form.Label>Last name</Form.Label>
            <Form.Control
              type="text"
              name="lastName"
              value={form.lastName}
              placeholder="Enter last name"
              onChange={setProperty}
              isInvalid={!!errors.lastName && !!touched.lastName}
              required
            />
            <Form.Control.Feedback type="invalid">
              {touched.lastName && errors.lastName}
            </Form.Control.Feedback>
          </Form.Group>

          <Button
            className="mt-2"
            variant="primary"
            disabled={submitting || Object.keys(errors).length > 0}
            type="submit"
          >
            {submitting ? "Registering..." : "Register"}
          </Button>
        </Form>
      </Card.Body>
    </Card>
  );
}
