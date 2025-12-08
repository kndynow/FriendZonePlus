import { useState } from "react";
import LoginForm from "./LoginForm";
import RegisterForm from "./RegisterForm";
import { Col, Row } from "react-bootstrap";

type View = "login" | "register";

export default function WelcomePage() {
  const [view, setView] = useState<View>("login");

  return (
    <>
      <Row className="w-100">
        <Col className="w-100">
          <h2>{view === "login" ? "Logga in" : "Skapa konto"}</h2>

          {view === "login" && (
            <LoginForm onSwitchToRegister={() => setView("register")} />
          )}

          {view === "register" && (
            <RegisterForm onSwitchToLogin={() => setView("login")} />
          )}
        </Col>
      </Row>
    </>
  );
}
