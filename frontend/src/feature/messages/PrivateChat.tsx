import { useState } from "react";
import SingleMessage from "./SingleMessage";
import { Col, Row } from "react-bootstrap";
import ChatInput from "./ChatInput";
export default function PrivateChat() {
  const [messages, setMessages] = useState([
    {
      id: 1,
      from: "other",
      content: "Hej! Vad gör du? 😊",
    },
    {
      id: 2,
      from: "me",
      content:
        "Inte så mycket, ligger i soffan och försöker bestämma om jag ska laga mat eller beställa något. Du då?",
    },
    {
      id: 3,
      from: "other",
      content:
        "Haha samma här! Har stirrat in i kylen tre gånger och hoppas att något magiskt ska dyka upp men nope 😂",
    },
    {
      id: 4,
      from: "me",
      content:
        "Känner igen det där. Jag har typ bara pasta, lite ost och… ett halvt paket körsbärstomater som börjar se tveksamma ut 🫠",
    },
    {
      id: 5,
      from: "other",
      content:
        "Det låter ändå som mer än vad jag har. Jag har bara yoghurt och en gammal paprika som jag inte riktigt vågar titta på längre 😭",
    },
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
