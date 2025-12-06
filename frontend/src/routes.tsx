import type { RouteObject } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import HomePage from "./pages/HomePage";
import RegisterPage from "./pages/RegisterPage";
import NotFoundPage from "./pages/NotFoundPage";
import UserPage from "./pages/UserPage";
import SettingsPage from "./pages/SettingsPage";
import FindFriendsPage from "./pages/FindFriendsPage";
import MessagesPage from "./pages/MessagesPage";

const routes: RouteObject[] = [
  {
    index: true,
    element: <LoginPage />,
    handle: { title: "Login" },
  },
  {
    index: true,
    element: <HomePage />,
    handle: { title: "Home" },
  },
  {
    path: "register",
    element: <RegisterPage />,
    handle: { title: "Register" },
  },
  {
    path: "user/:id",
    element: <UserPage />,
    handle: { type: "user" },
  },
  {
    path: "*",
    element: <NotFoundPage />,
    handle: { title: "Not Found" },
  },
  {
    path: "settings",
    element: <SettingsPage />,
    handle: { title: "Settings" },
  },
  {
    path: "findFriends",
    element: <FindFriendsPage />,
    handle: { title: "Find Friends" },
  },
  {
    path: "messages",
    element: <MessagesPage />,
    handle: { title: "Messages" },
  },
];

export default routes;
