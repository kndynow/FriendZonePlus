import { Col, Row } from "react-bootstrap";
import UserPreview from "./UserPreview";

type WallPostProps = {
  user: {
    fullName: string;
    imgPath?: string;
  };
  content: string;
  button?: {
    buttonIcon: string;
    onClick: () => void;
  };
  subtitle?: string;
};

export default function WallPost({
  user,
  content,
  button,
  subtitle,
}: WallPostProps) {
  return (
    <>
      <Row className="f-border f-shadow py-3 m-1 my-4 semi-transparent-bg">
        <Col>
          <div>
            <UserPreview
              fullName={user.fullName}
              imgPath={user.imgPath}
              subtitle={subtitle}
              button={button}
            />

            <div className="p-2">
              <p>{content}</p>
            </div>
          </div>
        </Col>
      </Row>
    </>
  );
}
