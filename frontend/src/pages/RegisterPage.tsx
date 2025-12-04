import { Button, Form, Card } from "react-bootstrap";
import { useState } from "react";
import { useAuth } from "../../context/AuthProvider";
import type { RegisterRequest } from "../../types/auth";

export default function RegisterPage() {
  const [form, setForm] = useState<RegisterRequest>({
    username: "",
    email: "",
    password: "",
    firstName: "",
    lastName: "",
  });

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  }

  async function handleRegister(event: React.FormEvent) {
    event.preventDefault();
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
              type="username"
              name="username"
              value={form.email}
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

          <Button className="mt-2" variant="primary">
            Register
          </Button>
        </Form>
      </Card.Body>
    </Card>
  );
}
