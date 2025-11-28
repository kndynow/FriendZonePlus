import type { RouteObject } from "react-router-dom";
import HomePage from "./pages/HomePage";
import RegisterPage from "./pages/RegisterPage";
import NotFoundPage from "./pages/NotFoundPage";

const routes: RouteObject[] = [
  { path: "homepage", element: <HomePage /> },
  { index: true, element: <RegisterPage /> },
  { path: "*", element: <NotFoundPage /> },
];

export default routes;
