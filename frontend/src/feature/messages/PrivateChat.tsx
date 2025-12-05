import { useState } from "react";
import SingleMessage from "./SingleMessage";
import { Col, Row } from "react-bootstrap";
import ChatInput from "./ChatInput";
export default function PrivateChat() {
  const [messages, setMessages] = useState([
    {
      id: 1,
      from: "other",
      content:
        "Lorem ipsum dolor sit amet.gergerggggggggg gggggggggggggggggggggggggggggggggggggggg ggggggggggggggggggggergerg..",
    },
    { id: 2, from: "me", content: "Ja" },
    { id: 3, from: "other", content: "Lorem ipsum dolor sit amet..." },
    { id: 4, from: "me", content: "Jo" },
    { id: 5, from: "other", content: "Lorem ipsum dolor sit amet..." },
  ]);

  const [input, setInput] = useState("");
  function sendMessage() {
    if (!input.trim()) return;
    setMessages([...messages, { id: Date.now(), from: "me", content: input }]);
    setInput("");
  }
  return (
    <>
      <div className="semi-transparent-bg f-border p-3">
        <Row>
          <Col className="d-flex flex-column gap-2">
            {messages.map((m) => (
              <SingleMessage key={m.id} from={m.from} content={m.content} />
            ))}
          </Col>
        </Row>
        <Row>
          <Col>
            <ChatInput
              input={input}
              setInput={setInput}
              sendMessage={sendMessage}
            />
          </Col>
        </Row>
      </div>
    </>
  );
}
