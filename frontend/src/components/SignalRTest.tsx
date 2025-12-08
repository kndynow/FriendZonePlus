import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

interface Message {
  id: number;
  senderId: number;
  receiverId: number;
  content: string;
  sentAt: string;
  isRead: boolean;
}

const SignalRTest: React.FC = () => {
  const [messages, setMessages] = useState<Message[]>([]);
  const [content, setContent] = useState("");
  const [receiverId, setReceiverId] = useState<number>(2);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("/hubs/message", {
        withCredentials: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveMessage", (message: Message) => {
      console.log("Received message:", message);
      setMessages((prev) => [...prev, message]);
    });

    connection
      .start()
      .then(() => console.log("Connected to SignalR hub"))
      .catch((err) => console.error("SignalR connection error:", err));

    return () => {
      connection.stop();
    };
  }, []);

  const sendMessage = async () => {
    if (!content.trim()) return;

    const res = await fetch("/api/Message/send", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({
        receiverId,
        content,
      }),
    });

    if (!res.ok) {
      console.error("Failed to send message");
      return;
    }

    setContent("");
  };

  return (
    <div>
      <h2>SignalR Test</h2>

      <div>
        <input
          type="number"
          value={receiverId}
          onChange={(e) => setReceiverId(Number(e.target.value))}
          placeholder="Receiver ID"
        />

        <input
          value={content}
          onChange={(e) => setContent(e.target.value)}
          placeholder="Type message..."
        />

        <button onClick={sendMessage}>Send</button>
      </div>

      <hr />

      <ul>
        {messages.map((msg) => (
          <li key={msg.id}>
            <strong>Sender {msg.senderId}</strong>: {msg.content} →{" "}
            <strong>{msg.receiverId}</strong>
            <em> ({new Date(msg.sentAt).toLocaleTimeString()})</em>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default SignalRTest;
