import { Outlet, useLocation } from "react-router-dom";
import { AuthProvider } from "../context/AuthProvider";
import { Toaster } from "react-hot-toast";
import NavigationBar from "./components/Navigation/NavigationBar";

function App() {
  // scroll to top when the route changes
  useLocation();
  window.scrollTo({ top: 0, left: 0, behavior: "instant" });
  return (
    <>
      <Toaster
        position="top-center"
        toastOptions={{ duration: 4000, removeDelay: 0 }}
      />
      <AuthProvider>
        <NavigationBar />
        {/* This is where pages render */}
        <Outlet />
      </AuthProvider>
    </>
  );
}

export default App;
