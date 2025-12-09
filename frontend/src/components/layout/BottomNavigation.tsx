import { Col, Row } from "react-bootstrap";
import { NavItem } from "./NavItem";
import { useAuth } from "../../../context/AuthProvider";

export default function BottomNavigation() {
  const { user } = useAuth();
  const userId = user?.id ?? "";

  const navItems = [
    { to: "/findFriends", icon: "search-heart" },
    { to: "/messages", icon: "chat-heart" },
    { to: "/", icon: "house-door" },
    { to: `/user/${userId}`, icon: "person" },
    { to: "/settings", icon: "gear" },
  ];
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
