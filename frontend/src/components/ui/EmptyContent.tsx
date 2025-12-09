import { Col, Row } from "react-bootstrap";

type EmptyContentProps = {
  header?: string;
  content?: string;
};

export default function EmptyContent({ header, content }: EmptyContentProps) {
  return (
    <>
      <Row className=" flex-grow-1 align-items-center text-center">
        <Col>
          <h5 className="">{header}</h5>
          <p>{content}</p>
        </Col>
      </Row>
    </>
  );
}
