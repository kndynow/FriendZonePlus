import { useState } from "react";
import LoginForm from "./LoginForm";
import RegisterForm from "./RegisterForm";
import { Col, Row } from "react-bootstrap";
import WelcomeHeader from "./WelcomeHeader";

type View = "login" | "register";

export default function WelcomePage() {
  const [view, setView] = useState<View>("login");

  return (
    <>
      <Row className="flex-grow-1 px-1">
        <Col xs={12}>
          <WelcomeHeader />
        </Col>
        <Col>
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
