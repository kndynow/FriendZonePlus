import "./UserPreview.css";
import { Button, Col, Row } from "react-bootstrap";

export default function UserPreview() {
  return (
    <>
      <Row className="align-items-center">
        <Col xs="auto">
          <img
            src="images/profilePlaceholder.png"
            className="profile-img f-shadow"
          />
        </Col>
        <Col>
          <h2>Här är namn</h2>
          <p>Här är optional text</p>
        </Col>
        <Col xs="auto">
          <Button>icon here</Button>
          <p>optional text</p>
        </Col>
      </Row>
    </>
  );
}
