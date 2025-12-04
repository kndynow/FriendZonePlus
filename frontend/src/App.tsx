import { Outlet, useLocation } from "react-router-dom";
import NavigationBar from "./components/Navigation/NavigationBar";

function App() {
  // scroll to top when the route changes
  useLocation();
  window.scrollTo({ top: 0, left: 0, behavior: "instant" });
  return (
    <>
      <div className="d-flex flex-column min-vh-100">
        <NavigationBar />
        {/* This is where pages render */}
        <Outlet />
      </div>
    </>
  );
}

export default App;
