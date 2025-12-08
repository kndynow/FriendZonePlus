import { Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthProvider";
import { Spinner } from "react-bootstrap";
import WelcomePage from "../src/feature/auth/WelcomePage";

export default function ProtectedLayout() {
  const { user, loading } = useAuth();

  if (loading) {
    return (
      <div className="d-flex justify-content-center mt-5">
        <Spinner animation="border" />
      </div>
    );
  }

  if (!user) {
    return <WelcomePage />;
  }

  return <Outlet />;
}
