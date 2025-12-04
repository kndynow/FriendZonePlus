import { Button, Form, Card } from "react-bootstrap";
import { useState } from "react";
import { useAuth } from "../../context/AuthProvider";
import type { RegisterRequest } from "../../types/auth";
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

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  }

  async function handleRegister(event: React.FormEvent) {
    event.preventDefault();

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

      if (err?.message) {
        toast.error(err.message, { id: "register-error" });
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
              required
            />
          </Form.Group>

          <Form.Group>
            <Form.Label>Username</Form.Label>
            <Form.Control
              type="text"
              name="username"
              value={form.username}
              placeholder="Enter username"
              onChange={setProperty}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              name="password"
              value={form.password}
              placeholder="Password"
              onChange={setProperty}
              required
            />
          </Form.Group>

          <Form.Group>
            <Form.Label>First name</Form.Label>
            <Form.Control
              type="text"
              name="firstName"
              value={form.firstName}
              placeholder="Enter first name"
              onChange={setProperty}
              required
            />
          </Form.Group>

          <Form.Group>
            <Form.Label>Last name</Form.Label>
            <Form.Control
              type="text"
              name="lastName"
              value={form.lastName}
              placeholder="Enter last name"
              onChange={setProperty}
              required
            />
          </Form.Group>

          <Button
            className="mt-2"
            variant="primary"
            disabled={submitting}
            type="submit"
          >
            {submitting ? "Registering..." : "Register"}
          </Button>
        </Form>
      </Card.Body>
    </Card>
  );
}
