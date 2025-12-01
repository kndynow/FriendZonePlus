import { Link } from "react-router-dom";
import { Button, Row } from "react-bootstrap";

export default function HomePage() {
  return (
    <div
      className="d-flex justify-content-center align-items-center"
      style={{ height: "100vh" }}
    >
      <Link to="/register">Go to register page</Link>
    </div>
  );
}
