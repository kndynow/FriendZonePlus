import "./UserPreview.css";
import { Button, Col, Row } from "react-bootstrap";

type UserPreviewProps = {
  imgPath?: string;
  fullName: string;
  description?: string;
  buttonIcon?: string;
  onClick?: () => void;
  underButtonText?: string;
};

export default function UserPreview({
  imgPath,
  fullName,
  description,
  buttonIcon,
  onClick,
  underButtonText,
}: UserPreviewProps) {
  return (
    <>
      <Row className="align-items-top p-4">
        <Col xs="auto">
          <img
            src={imgPath || "images/profilePlaceholder.png"}
            className="profile-img f-shadow"
          />
        </Col>
        <Col>
          <h2>{fullName}</h2>
          <p>{description}</p>
        </Col>
        {(buttonIcon || underButtonText) && (
          <Col xs="auto">
            {buttonIcon && <Button onClick={onClick}>{buttonIcon}</Button>}

            {underButtonText && <p>{underButtonText}</p>}
          </Col>
        )}
      </Row>
    </>
  );
}
