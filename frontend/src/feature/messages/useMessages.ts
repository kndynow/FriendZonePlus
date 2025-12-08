import { useState } from "react";
import { toast } from "react-hot-toast";
import type { BackendMessage, MappedMessage } from "../../../types/message";

export function useMessages() {
  const [messages, setMessages] = useState<MappedMessage[]>([]);

  const mapMessage = (
    message: BackendMessage,
    currentUserId: number
  ): MappedMessage => ({
    id: message.id,
    from: message.senderId === currentUserId ? "me" : "other",
    content: message.content,
  });

  const mapMessages = (list: BackendMessage[], currentUserId: number) =>
    list.map((message) => mapMessage(message, currentUserId));

  const getConversation = async (receiverId: number, currentUserId: number) => {
    try {
      const res = await fetch(`/api/Message/conversation/${receiverId}`, {
        method: "GET",
        credentials: "include",
      });

      if (!res.ok) {
        toast.error("Could not load messages");
        return;
      }

      const data: BackendMessage[] = await res.json();
      setMessages(mapMessages(data, currentUserId));
    } catch {
      toast.error("Network error, please try again later");
    }
  };

  const sendMessage = async (
    receiverId: number,
    content: string,
    currentUserId: number
  ) => {
    try {
      const res = await fetch(`/api/Message/send`, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ receiverId, content }),
      });

      if (!res.ok) {
        toast.error("Could not send message");
        return;
      }

      const data: BackendMessage = await res.json();

      setMessages((prev) => [...prev, mapMessage(data, currentUserId)]);
    } catch {
      toast.error(
        "Network error. Could not send message, please try again later"
      );
    }
  };

  return {
    messages,
    getConversation,
    sendMessage,
  };
}
