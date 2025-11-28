import { Outlet } from "react-router-dom";

function App() {
  return (
    <>
      <div className="d-flex flex-column min-vh-100">
        {/* This is where pages render */}
        <Outlet />
      </div>
    </>
  );
}

export default App;
