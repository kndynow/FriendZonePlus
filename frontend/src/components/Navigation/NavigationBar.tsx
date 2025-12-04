import { Col, Row } from "react-bootstrap";
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
    <Row className="bottom-nav f-shadow">
      {navItems.map((navItem) => (
        <Col className="d-flex justify-content-center align-items-center">
          <NavItem key={navItem.to} to={navItem.to} icon={navItem.icon} />
        </Col>
      ))}
    </Row>
  );
}
