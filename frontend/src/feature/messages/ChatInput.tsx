import { Button, Col, Row } from "react-bootstrap";
import FormField from "../../components/ui/FormField";

type ChatInputProps = {
  input: string;
  setInput: (v: string) => void;
  sendMessage: () => void;
};

export default function ChatInput({
  input,
  setInput,
  sendMessage,
}: ChatInputProps) {
  return (
    <>
      <Row className="justify-content-end align-items-top pt-4">
        <Col className="pe-0">
          <FormField
            placeholder="Write something..."
            value={input}
            onChange={(event) => setInput(event.target.value)}
            onKeyDown={(e) => e.key === "Enter" && sendMessage()}
          ></FormField>
        </Col>
        <Col xs="auto" className="ps-0">
          <Button onClick={sendMessage}>Send</Button>
        </Col>
      </Row>
    </>
  );
}
