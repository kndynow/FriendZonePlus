import { Outlet, useLocation } from "react-router-dom";
import { Toaster } from "react-hot-toast";
import NavigationBar from "./components/layout/BottomNavigation";
import Header from "./components/layout/Header";
import { useAuth } from "../context/AuthProvider";

function App() {
  useLocation();
  const { user } = useAuth();

  const hideLayout = !user;

  window.scrollTo({ top: 0, left: 0, behavior: "instant" });
  return (
    <>
      <div className="pastel-bg"></div>
      <div className="layout-wrapper container-fluid">
        {!hideLayout && <Header />}
        <main className={`page-content ${!user ? "no-margins" : ""}`}>
          <Toaster
            position="top-center"
            toastOptions={{ duration: 4000, removeDelay: 0 }}
          />
          <Outlet />
        </main>
        {!hideLayout && <NavigationBar />}
      </div>
    </>
  );
}

export default App;
