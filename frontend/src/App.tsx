import { Outlet, useLocation } from "react-router-dom";
import { Toaster } from "react-hot-toast";
import NavigationBar from "./components/layout/BottomNavigation";
import Header from "./components/layout/Header";
import { Container } from "react-bootstrap";

function App() {
  useLocation();
  window.scrollTo({ top: 0, left: 0, behavior: "instant" });
  return (
    <>
      <div className="pastel-bg"></div>
      <div className="layout-wrapper">
        <Header />
        <Container>
          <main className="page-content">
            <Toaster
              position="top-center"
              toastOptions={{ duration: 4000, removeDelay: 0 }}
            />
            <Outlet />
          </main>
        </Container>
        <NavigationBar />
      </div>
    </>
  );
}

export default App;
