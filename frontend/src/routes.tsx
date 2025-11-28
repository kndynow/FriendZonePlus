import type { RouteObject } from "react-router-dom";
import HomePage from "./pages/HomePage";
import RegisterPage from "./pages/RegisterPage";
import NotFoundPage from "./pages/NotFoundPage";

const routes: RouteObject[] = [
  { index: true, element: <HomePage /> },
  { path: "register", element: <RegisterPage /> },
  { path: "*", element: <NotFoundPage /> },
];

export default routes;
