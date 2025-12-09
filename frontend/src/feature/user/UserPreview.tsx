import { Col, Row } from "react-bootstrap";
import ProfileImage from "../../components/ui/ProfileImage";

type UserPreviewProps = {
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
  imgPath,
  fullName,
  subtitle,
  className,
  button,
}: UserPreviewProps) {
  return (
    <>
      <Row className={`align-items-center p-2 ${className}`}>
        <Col xs="auto">
          <ProfileImage imgPath={imgPath} />
        </Col>
        <Col className="d-flex flex-column justify-content-center">
          <h5 className="m-0">{fullName}</h5>
          <p>{subtitle}</p>
        </Col>
        {button?.buttonIcon && (
          <Col
            xs="auto"
            className="d-flex flex-column align-items-end justify-content-center"
          >
            {button.buttonIcon && (
              <button onClick={button.onClick} className="f-button">
                {button.buttonIcon}
              </button>
            )}
          </Col>
        )}
      </Row>
    </>
  );
}
