import { Col, Row } from "react-bootstrap";

type UserPreviewProps = {
  imgPath?: string;
  fullName: string;
  subtitle?: string;
  button?: {
    buttonIcon: string;
    onClick: () => void;
  };
};

export default function UserPreview({
  imgPath,
  fullName,
  subtitle,
  button,
}: UserPreviewProps) {
  return (
    <>
      <Row className="align-items-start p-2">
        <Col xs="auto">
          <img
            src={imgPath || "/images/profilePlaceholder.png"}
            className="profile-img f-shadow"
          />
        </Col>
        <Col className="d-flex flex-column justify-content-center">
          <h5 className="m-0">{fullName}</h5>
          <p>{subtitle}</p>
        </Col>
        {button?.buttonIcon && (
          <Col
            xs="auto"
            className="d-flex flex-column align-items-end justify-content-start"
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
