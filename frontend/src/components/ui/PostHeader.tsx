import { Col, Row } from "react-bootstrap";
import ProfileImage from "./ProfileImage";
import ActionDropdown, { type ActionButton } from "./ActionDropdown";

type PostHeaderProps = {
  user: {
    fullName: string;
    imgPath?: string;
  };
  subtitle?: string;
  actions?: ActionButton[];
};

export default function PostHeader({
  user,
  subtitle,
  actions,
}: PostHeaderProps) {
  return (
    <Row className="align-items-start p-2 ps-3 m-0">
      <Col xs="auto" className="p-0">
        <ProfileImage imgPath={user.imgPath} />
      </Col>
      <Col className="d-flex flex-column justify-content-center">
        <h5 className="fs-5 mb-1">{user.fullName}</h5>
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

