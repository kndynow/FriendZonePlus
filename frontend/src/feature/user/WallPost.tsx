import { Col, Row } from "react-bootstrap";
import UserPreview from "./UserPreview";

type WallPostProps = {
  user: {
    fullName: string;
    imgPath?: string;
  };
  content: string;
  buttonIcon?: string;
  subtitle?: string;
};

export default function WallPost({
  user,
  content,
  buttonIcon,
  subtitle,
}: WallPostProps) {
  return (
    <>
      <Row className="f-border f-shadow py-2 m-1 my-4">
        <Col>
          <div>
            <UserPreview
              fullName={user.fullName}
              imgPath={user.imgPath}
              subtitle={subtitle}
              buttonIcon={buttonIcon}
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
