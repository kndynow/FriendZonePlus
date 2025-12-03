import "./NavigationBar.css";
import { NavItem } from "./NavItem";

//TODO: change to real pages
const navItems = [
  { to: "/register", icon: "search-heart" },
  { to: "/chat", icon: "chat-heart" },
  { to: "/", icon: "house-door" },
  { to: "/profile", icon: "person" },
  { to: "/settings", icon: "gear" },
];

export default function NavigationBar() {
  return (
    <div className="bottom-nav">
      {navItems.map((navItem) => (
        <NavItem key={navItem.to} to={navItem.to} icon={navItem.icon} />
      ))}
    </div>
  );
}
