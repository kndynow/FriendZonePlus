import { Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthProvider";
import { Spinner } from "react-bootstrap";
import { Navigate, useLocation } from "react-router-dom";

export default function ProtectedLayout() {
  const { user, loading } = useAuth();
  const location = useLocation();

  if (loading) {
    return (
      <div className="d-flex justify-content-center mt-5">
        <Spinner animation="border" />
      </div>
    );
  }

  if (!user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <Outlet />;
}