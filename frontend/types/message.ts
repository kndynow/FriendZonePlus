export type MappedMessage = {
  id: number;
  from: "me" | "other";
  content: string;
};

export type BackendMessage = {
  id: number;
  senderId: number;
  receiverId: number;
  content: string;
  sentAt: string;
  isRead: boolean;
};
