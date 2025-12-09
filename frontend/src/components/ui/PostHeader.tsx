import { Col, Row } from "react-bootstrap";
import { Link } from "react-router-dom";
import ProfileImage from "./ProfileImage";
import ActionDropdown, { type ActionButton } from "./ActionDropdown";

type PostHeaderProps = {
  user: {
    fullName: string;
    imgPath?: string;
  };
  subtitle?: string;
  actions?: ActionButton[];
  userId?: number;
};

export default function PostHeader({
  user,
  subtitle,
  actions,
  userId,
}: PostHeaderProps) {
  const profileLink = userId ? `/user/${userId}` : undefined;
  const linkStyle = profileLink ? { cursor: 'pointer' } : {};

  return (
    <Row className="align-items-start p-2 ps-3 m-0">
      <Col xs="auto" className="p-0">
        {profileLink ? (
          <Link to={profileLink} style={linkStyle}>
            <ProfileImage imgPath={user.imgPath} />
          </Link>
        ) : (
          <ProfileImage imgPath={user.imgPath} />
        )}
      </Col>
      <Col className="d-flex flex-column justify-content-center">
        {profileLink ? (
          <Link to={profileLink} style={{ ...linkStyle, textDecoration: 'none', color: 'inherit' }}>
            <h5 className="fs-5 mb-1">{user.fullName}</h5>
          </Link>
        ) : (
          <h5 className="fs-5 mb-1">{user.fullName}</h5>
        )}
        {subtitle && <p className="mb-2">{subtitle}</p>}
      </Col>
      {actions && actions.length > 0 && (
        <Col
          xs="auto"
          className="d-flex align-items-start justify-content-end mb-4 pb-2"
        >
          <ActionDropdown actions={actions} />
        </Col>
      )}
    </Row>
  );
}

