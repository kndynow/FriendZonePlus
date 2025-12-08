import { useEffect, useRef, useState } from "react";
import SingleMessage from "./SingleMessage";
import { Col, Row } from "react-bootstrap";
import ChatInput from "./ChatInput";

const initialMessages = [
  { id: 1, from: "other", content: "Hej! Vad gör du? 😊" },
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
];

export default function PrivateChat() {
  const [messages, setMessages] = useState(initialMessages);
  const [input, setInput] = useState("");

  const scrollRef = useRef<HTMLDivElement | null>(null);

  const scrollToBottom = () => {
    const container = scrollRef.current;
    if (!container) return;
    container.scrollTop = container.scrollHeight;
  };

  useEffect(scrollToBottom, [messages]);

  const sendMessage = () => {
    if (!input.trim()) return;

    setMessages((prev) => [
      ...prev,
      { id: Date.now(), from: "me", content: input },
    ]);

    setInput("");
  };
  return (
    <>
      <div className="semi-transparent-bg f-border p-3 d-flex flex-column">
        <Row ref={scrollRef} className="flex-grow-1 overflow-auto">
          <Col className="d-flex flex-column gap-2">
            {messages.map((message) => (
              <SingleMessage
                key={message.id}
                from={message.from}
                content={message.content}
              />
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
