import { Outlet, useLocation } from "react-router-dom";
import { AuthProvider } from "../context/AuthProvider";
import { Toaster } from "react-hot-toast";

function App() {
  // scroll to top when the route changes
  useLocation();
  window.scrollTo({ top: 0, left: 0, behavior: "instant" });
  return (
    <>
      <div className="d-flex flex-column min-vh-100">
        <Toaster
          position="top-center"
          toastOptions={{ duration: 4000, removeDelay: 0 }}
        />
        <AuthProvider>
          {/* This is where pages render */}
          <Outlet />
        </AuthProvider>
      </div>
    </>
  );
}

export default App;
