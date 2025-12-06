import { Col, Row } from "react-bootstrap";
import BackButton from "../ui/BackButton";
import { useMatches } from "react-router-dom";
import UserPreview from "../../feature/user/UserPreview";

export default function Header() {
  const fakeUser = {
    fullName: "Anna Andersson",
  };

  const matches = useMatches();
  const match = matches[matches.length - 1];

  const handle = (match.handle as any) || {};
  const isUserHeader = handle.type === "user";
  const title = handle.title || "FriendZone+";

  if (isUserHeader) {
    return (
      <Row className="top-nav f-shadow align-items-center semi-transparent-bg">
        <Col xs="auto">
          <BackButton />
        </Col>

        <Col>
          <UserPreview fullName={fakeUser.fullName} />
        </Col>
      </Row>
    );
  }
  return (
    <>
      <Row className="top-nav f-shadow align-items-center semi-transparent-bg">
        <Col xs="auto">
          <BackButton />
        </Col>
        <Col>
          <h1>{title}</h1>
        </Col>
      </Row>
    </>
  );
}
