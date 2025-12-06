import { Button, Form, Card } from "react-bootstrap";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthProvider";
import toast from "react-hot-toast";

export default function LoginPage() {
  const [form, setForm] = useState({ usernameOrEmail: "", password: "" });
  const [submitting, setSubmitting] = useState(false);
  const navigate = useNavigate();
  const { login } = useAuth();

  function setProperty(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  async function handleLogin(e: React.FormEvent) {
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
    <Card className="mt-5 mx-2">
      <Card.Body>
        <Card.Title className="text-center">Log in</Card.Title>

        <Form onSubmit={handleLogin}>
          <Form.Group className="mb-3">
            <Form.Label>Username or Email</Form.Label>
            <Form.Control
              type="text"
              name="usernameOrEmail"
              value={form.usernameOrEmail}
              placeholder="Username or email"
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

          <Button
            className="mt-2"
            variant="primary"
            disabled={submitting}
            type="submit"
          >
            {submitting ? "Logging in..." : "Log in"}
          </Button>
        </Form>
      </Card.Body>
    </Card>
  );
}
