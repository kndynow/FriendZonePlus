import { Col, Row } from "react-bootstrap";
import BackButton from "./BackButton";
import { useMatches } from "react-router-dom";
import UserPreview from "../UserPreview";

export default function Header() {
  const fakeUser = {
    fullName: "Anna Andersson",
  };

  const matches = useMatches();
  const match = matches[matches.length - 1];
  const title =
    (match.handle as { title?: string } | undefined)?.title || "FriendZone+";
  if ((match.handle as any)?.type === "user") {
    return (
      <Row className="top-nav f-shadow align-items-center">
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
      <Row className="top-nav f-shadow align-items-center">
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
