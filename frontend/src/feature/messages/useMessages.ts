import { useState } from "react";
import { toast } from "react-hot-toast";
import type { BackendMessage, MappedMessage } from "../../../types/message";

export function useMessages() {
  const [messages, setMessages] = useState<MappedMessage[]>([]);

  const mapMessages = (
    data: BackendMessage[],
    currentUserId: number
  ): MappedMessage[] =>
    data.map((message) => ({
      id: message.id,
      from: message.senderId === currentUserId ? "me" : "other",
      content: message.content,
    }));

  const getConversation = async (receiverId: number, currentUserId: number) => {
    try {
      const res = await fetch(`/api/Message/conversation/${receiverId}`, {
        method: "GET",
        credentials: "include",
      });

      if (!res.ok) {
        toast.error("Kunde inte hämta meddelanden.");
        return;
      }

      const data: BackendMessage[] = await res.json();
      const mapped = mapMessages(data, currentUserId);

      setMessages(mapped);
    } catch {
      toast.error("Serverfel — försök igen senare.");
    }
  };

  return {
    messages,
    getConversation,
    setMessages,
  };
}
