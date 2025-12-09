import { Button, Col, Row } from "react-bootstrap";
import FormField from "../../components/ui/FormField";

type ChatInputProps = {
  input: string;
  setInput: (v: string) => void;
  sendMessage: () => void;
  maxLength?: number;
};

export default function ChatInput({
  input,
  setInput,
  sendMessage,
  maxLength,
}: ChatInputProps) {
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    if (maxLength === undefined || value.length <= maxLength) {
      setInput(value);
    } else {
    }
  };

  return (
    <>
      <Row className="justify-content-end align-items-center mt-2">
        <Col className="pe-1 mb-0 pb-0">
          <FormField
            placeholder="Write something..."
            value={input}
            onChange={handleChange}
            onKeyDown={(e) => e.key === "Enter" && sendMessage()}
          ></FormField>
        </Col>

        <Col xs="auto" className="ps-0">
          <Button onClick={sendMessage}>Send</Button>
        </Col>
      </Row>
      <Row>
        {maxLength && (
          <Col className="text-start small text-muted mb-1 ps-3">
            {input.length}/{maxLength}
          </Col>
        )}
      </Row>
    </>
  );
}
