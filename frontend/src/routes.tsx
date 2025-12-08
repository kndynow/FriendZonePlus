import type { RouteObject } from "react-router-dom";
import HomePage from "./pages/HomePage";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import NotFoundPage from "./pages/NotFoundPage";
import UserPage from "./pages/UserPage";
import SettingsPage from "./pages/SettingsPage";
import FindFriendsPage from "./pages/FindFriendsPage";
import MessagesPage from "./feature/messages/MessagesPage";
import ProtectedLayout from "../utils/ProtectedLayout";
import PrivateChat from "./feature/messages/PrivateChat";

const routes: RouteObject[] = [
  { path: "login", element: <LoginPage />, handle: { title: "Login" } },
  {
    path: "register",
    element: <RegisterPage />,
    handle: { title: "Register" },
  },

  {
    path: "/",
    element: <ProtectedLayout />,
    children: [
      { index: true, element: <HomePage />, handle: { title: "Home" } },
      { path: "user/:id", element: <UserPage />, handle: { type: "user" } },
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
      {
        path: "messages/:id",
        element: <PrivateChat />,
        handle: { type: "user" },
      },
    ],
  },

  { path: "*", element: <NotFoundPage />, handle: { title: "Not Found" } },
];

export default routes;
