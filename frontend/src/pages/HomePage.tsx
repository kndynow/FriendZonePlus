import { Link } from "react-router-dom";
import UserPreview from "../components/UserPreview";
import { Row } from "react-bootstrap";

export default function HomePage() {
  return (
    <>
      <Row>
        <div>
          <Link to="/register">Go to register page</Link>
        </div>
        <UserPreview></UserPreview>
      </Row>
    </>
  );
}
