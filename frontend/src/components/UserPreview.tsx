import "./UserPreview.css";
import { Button, Col, Row } from "react-bootstrap";

type UserPreviewProps = {
  imgPath?: string;
  fullName: string;
  messagePreview?: string;
  buttonIcon?: string;
  onClick?: () => void;
  timeStamp?: string;
};

export default function UserPreview({
  imgPath,
  fullName,
  messagePreview,
  buttonIcon,
  onClick,
  timeStamp,
}: UserPreviewProps) {
  return (
    <>
      <Row className="align-items-center p-1">
        <Col xs="auto">
          <img
            src={imgPath || "/images/profilePlaceholder.png"}
            className="profile-img f-shadow"
          />
        </Col>
        <Col>
          <h5>{fullName}</h5>
          <p>{messagePreview}</p>
        </Col>
        {(buttonIcon || timeStamp) && (
          <Col xs="auto">
            {buttonIcon && <Button onClick={onClick}>{buttonIcon}</Button>}

            {timeStamp && <p>{timeStamp}</p>}
          </Col>
        )}
      </Row>
    </>
  );
}
