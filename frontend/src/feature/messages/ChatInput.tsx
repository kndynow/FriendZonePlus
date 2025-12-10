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
      <Row className="justify-content-end align-items-start mt-2">
        <Col className="pe-1 mb-0 pb-2">
          <FormField
            rows={2}
            placeholder="Write something..."
            value={input}
            onChange={handleChange}
            onKeyDown={(e) => e.key === "Enter" && sendMessage()}
          ></FormField>
          {maxLength && (
            <Col className="text-start small text-muted mb-1 ps-3">
              {input.length}/{maxLength}
            </Col>
          )}
        </Col>

        <Col xs="auto" className="ps-0 mt-2">
          <Button onClick={sendMessage} className="py-2 mt-1">
            Send
          </Button>
        </Col>
      </Row>
    </>
  );
}
