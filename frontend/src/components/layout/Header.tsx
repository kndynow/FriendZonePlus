import { Col, Row } from "react-bootstrap";
import BackButton from "../ui/BackButton";
import { useMatches, useParams } from "react-router-dom";
import UserPreview from "../../feature/user/UserPreview";
import { useUserProfile } from "../../hooks/useUserProfile";

export default function Header() {
  const { id } = useParams();
  const { profile } = useUserProfile(Number(id));

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

        <Col className="p-0">
          <UserPreview
            userId={Number(id)}
            imgPath={profile.profilePictureUrl}
            fullName={`${profile.firstName} ${profile.lastName}`}
          />
        </Col>
      </Row>
    );
  }

  return (
    <Row className="top-nav f-shadow align-items-center semi-transparent-bg">
      <Col xs="auto">
        <BackButton />
      </Col>
      <Col>
        <h1>{title}</h1>
      </Col>
    </Row>
  );
}
