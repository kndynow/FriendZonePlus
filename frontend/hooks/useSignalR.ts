import { useEffect } from "react";
import * as signalR from "@microsoft/signalr";

export function useSignalR(onMessageReceived: (msg: any) => void) {
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("/hubs/message", { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveMessage", (message) => {
      onMessageReceived(message);
    });

    connection
      .start()
      .then(() => console.log("SignalR connected"))
      .catch((err) => console.error("SignalR connection failed:", err));

    return () => {
      connection.stop();
    };
  }, [onMessageReceived]);
}
