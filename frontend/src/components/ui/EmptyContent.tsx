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
          <p className="m-0 fw-light">{header}</p>
          <p className="fst-italic fs-5 fw-semibold">{content}</p>
        </Col>
      </Row>
    </>
  );
}
