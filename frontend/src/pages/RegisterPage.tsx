import { Button, Form, Row, Container } from "react-bootstrap";

export default function RegisterPage() {
  return (
    <Container>
      <Row>
        <Form>
          <Form.Group>
            <Form.Label>Email</Form.Label>
            <Form.Control type="email" placeholder="Enter email" />
            <Button variant="primary">Primary</Button>
          </Form.Group>
        </Form>
      </Row>
    </Container>
  );
}
