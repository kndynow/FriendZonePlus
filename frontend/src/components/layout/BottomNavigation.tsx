import { Col, Row } from "react-bootstrap";
import { NavItem } from "./NavItem";

//TODO: change to real pages
const navItems = [
  { to: "/findFriends", icon: "search-heart" },
  { to: "/messages", icon: "chat-heart" },
  { to: "/", icon: "house-door" },
  { to: "/user/2", icon: "person" },
  { to: "/settings", icon: "gear" },
];

export default function BottomNavigation() {
  return (
    <Row className="bottom-nav f-shadow semi-transparent-bg">
      {navItems.map((navItem) => (
        <Col
          className="d-flex justify-content-center align-items-center"
          key={navItem.to}
        >
          <NavItem to={navItem.to} icon={navItem.icon} />
        </Col>
      ))}
    </Row>
  );
}
