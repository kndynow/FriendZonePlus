import { useEffect, useRef, useState } from "react";
import SingleMessage from "./SingleMessage";
import { Col, Row } from "react-bootstrap";
import ChatInput from "./ChatInput";
import { useMessages } from "./useMessages";
import { useAuth } from "../../../context/AuthProvider";
import { useParams } from "react-router-dom";
import EmptyContent from "../../components/ui/EmptyContent";

export default function PrivateChat() {
  const { messages, getConversation, sendMessage } = useMessages();
  const { user } = useAuth();

  const [input, setInput] = useState("");

  const { id } = useParams();
  const receiverId = Number(id);

  const scrollRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    scrollRef.current?.scrollTo(0, scrollRef.current.scrollHeight);
  }, [messages]);

  useEffect(() => {
    if (!user) return;
    getConversation(receiverId, user.id);
  }, [user, receiverId]);

  const handleSend = () => {
    if (!input.trim() || !user) return;

    sendMessage(receiverId, input);
    setInput("");
  };

  return (
    <>
      <div className="semi-transparent-bg f-border pt-1 px-3 d-flex flex-column flex-grow-1">
        <Row
          ref={scrollRef}
          className="flex-grow-1 overflow-auto f-border f-shadow-inset py-3"
        >
          {messages.length === 0 ? (
            <EmptyContent
              header="Get out of the friendzone..."
              content="Say hi!"
            />
          ) : (
            <Col className="d-flex flex-column gap-2">
              {messages.map((message) => (
                <SingleMessage
                  key={message.id}
                  from={message.from}
                  content={message.content}
                />
              ))}
            </Col>
          )}
        </Row>
        <Row>
          <Col>
            <ChatInput
              input={input}
              setInput={setInput}
              sendMessage={handleSend}
              maxLength={300}
            />
          </Col>
        </Row>
      </div>
    </>
  );
}
