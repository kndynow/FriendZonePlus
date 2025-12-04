import { Col, Row } from "react-bootstrap";
import BackButton from "./BackButton";

export default function Header() {
  return (
    <>
      <Row className="top-nav f-shadow">
        <Col xs="auto">
          <BackButton />
        </Col>
        <Col>
          <h1>Header</h1>
        </Col>
      </Row>
    </>
  );
}
