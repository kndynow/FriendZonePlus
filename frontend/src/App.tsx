import { Outlet, useLocation } from "react-router-dom";
import { Toaster } from "react-hot-toast";
import NavigationBar from "./components/layout/BottomNavigation";
import Header from "./components/layout/Header";
import { Container } from "react-bootstrap";
import { useAuth } from "../context/AuthProvider";

function App() {
  useLocation();
  const { user } = useAuth();

  // Hides Header and NavBar for login & register pages if the user is logged out
  const isAuthPage =
    location.pathname === "/login" || location.pathname === "/register";

  const hideLayout = !user && isAuthPage;

  window.scrollTo({ top: 0, left: 0, behavior: "instant" });
  return (
    <>
      <div className="pastel-bg"></div>
      <div className="layout-wrapper">
        {!hideLayout && <Header />}
        <Container>
          <main className="page-content">
            <Toaster
              position="top-center"
              toastOptions={{ duration: 4000, removeDelay: 0 }}
            />
            <Outlet />
          </main>
        </Container>
        {!hideLayout && <NavigationBar />}
      </div>
    </>
  );
}

export default App;
