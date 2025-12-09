import { useState, useCallback } from "react";
import { toast } from "react-hot-toast";
import type { BackendMessage, MappedMessage } from "../../../types/message";
import { useNavigate } from "react-router-dom";
import { useSignalR } from "../../../hooks/useSignalR";
import { useAuth } from "../../../context/AuthProvider";

export function useMessages() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [messages, setMessages] = useState<MappedMessage[]>([]);

  // Maps backend message to frontend format
  const mapMessage = useCallback(
    (message: BackendMessage, currentUserId: number): MappedMessage => ({
      id: message.id,
      from: message.senderId === currentUserId ? "me" : "other",
      content: message.content,
    }),
    []
  );

  const mapMessages = useCallback(
    (list: BackendMessage[], currentUserId: number) =>
      list.map((message) => mapMessage(message, currentUserId)),
    [mapMessage]
  );

  const handleIncoming = useCallback(
    (incoming: BackendMessage) => {
      setMessages((prev) => {
        if (prev.some((m) => m.id === incoming.id)) return prev;
        return [...prev, mapMessage(incoming, user?.id ?? -1)];
      });
    },
    [mapMessage, user?.id]
  );

  // Initializes SignalR connection and sets up incoming message handler
  useSignalR(handleIncoming);

  const getConversation = async (receiverId: number, currentUserId: number) => {
    try {
      const res = await fetch(`/api/Message/conversation/${receiverId}`, {
        method: "GET",
        credentials: "include",
      });

      if (!res.ok) {
        toast.error("Could not load messages");
        navigate("/messages");
        return;
      }

      const data: BackendMessage[] = await res.json();
      setMessages(mapMessages(data, currentUserId));
    } catch {
      toast.error("Network error, please try again later");
    }
  };

  const sendMessage = async (receiverId: number, content: string) => {
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
