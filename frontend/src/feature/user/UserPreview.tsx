import { Col, Row } from "react-bootstrap";
import ProfileImage from "../../components/ui/ProfileImage";
import { Link } from "react-router-dom";

type UserPreviewProps = {
  userId?: number;
  imgPath?: string;
  fullName: string;
  subtitle?: string;
  className?: string;
  button?: {
    buttonIcon: string;
    onClick: () => void;
  };
};

export default function UserPreview({
  userId,
  imgPath,
  fullName,
  subtitle,
  className,
  button,
}: UserPreviewProps) {
  return (
    <>
      <Row className={`align-items-start p-2 m-0 ${className}`}>
        <Col xs="auto" className="p-0">
          <ProfileImage imgPath={imgPath} />
        </Col>
        <Col className="d-flex flex-column justify-content-center mx-1">
          <h5 className="fs-5 mb-1">
            {userId ? (
              <Link className="clean-link" to={`/user/${userId}`}>
                {fullName}
              </Link>
            ) : (
              fullName
            )}
          </h5>
          <p className="mb-2 fs-6">{subtitle}</p>
        </Col>
        {button?.buttonIcon && (
          <Col
            xs="auto"
            className="d-flex flex-column align-items-end justify-content-start mb-4 pb-2"
          >
            {button.buttonIcon && (
              <button onClick={button.onClick} className="f-button fs-4">
                {button.buttonIcon}
              </button>
            )}
          </Col>
        )}
      </Row>
    </>
  );
}
