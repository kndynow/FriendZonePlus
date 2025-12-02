import { Button, Form, Card, Row } from "react-bootstrap";

async function handleRegister(event: React.FormEvent) {
  event.preventDefault();
}

export default function RegisterPage() {
  return (
    <Card className="mt-5 mx-2">
      <Card.Body>
        <Card.Title className="text-center">Register</Card.Title>

        <Form onSubmit={handleRegister}>
          <Form.Group>
            <Form.Label>Email</Form.Label>
            <Form.Control type="email" placeholder="Enter email" />
          </Form.Group>

          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control type="password" placeholder="Password" />
          </Form.Group>

          <Form.Group>
            <Form.Label>First name</Form.Label>
            <Form.Control type="text" placeholder="Enter first name" />
          </Form.Group>

          <Form.Group>
            <Form.Label>Last name</Form.Label>
            <Form.Control type="text" placeholder="Enter last name" />
          </Form.Group>

          <Button className="mt-2" variant="primary">
            Register
          </Button>
        </Form>
      </Card.Body>
    </Card>
  );
}
