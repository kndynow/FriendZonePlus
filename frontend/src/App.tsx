import { Outlet, useLocation } from "react-router-dom";
import { AuthProvider } from "../context/AuthProvider";

function App() {
  // scroll to top when the route changes
  useLocation();
  window.scrollTo({ top: 0, left: 0, behavior: "instant" });
  return (
    <>
      <div className="d-flex flex-column min-vh-100">
        <AuthProvider>
          {/* This is where pages render */}
          <Outlet />
        </AuthProvider>
      </div>
    </>
  );
}

export default App;
