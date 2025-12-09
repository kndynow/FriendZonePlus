import { Col, Row } from "react-bootstrap";
import type { ReactNode } from "react";
import PostHeader from "../../components/ui/PostHeader";
import type { ActionButton } from "../../components/ui/ActionDropdown";

type WallPostProps = {
  user: {
    fullName: string;
    imgPath?: string;
  };
  content: string | ReactNode;
  actions?: ActionButton[];
  subtitle?: string;
  userId?: number;
};

export default function WallPost({
  user,
  content,
  actions,
  subtitle,
  userId,
}: WallPostProps) {
  return (
    <Row className="f-border f-shadow py-2 pb-3 m-1 my-4 semi-transparent-bg">
      <Col>
        <div>
          <PostHeader user={user} subtitle={subtitle} actions={actions} userId={userId} />
          <div className="divider"></div>
          <div className="p-2">
            {typeof content === 'string' ? <p>{content}</p> : content}
          </div>
        </div>
      </Col>
    </Row>
  );
}
