import { useState } from "react";
import LoginForm from "./LoginForm";
import RegisterForm from "./RegisterForm";
import { Col, Row } from "react-bootstrap";

type View = "login" | "register";

export default function WelcomePage() {
  const [view, setView] = useState<View>("login");

  return (
    <>
      <Row>
        <Col>
          <h2 className="py-3">{view === "login" ? "Sign in" : "Sign up"}</h2>

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
