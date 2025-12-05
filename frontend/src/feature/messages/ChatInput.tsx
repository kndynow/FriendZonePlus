type ChatInputProps = {
  input: string;
  setInput: (v: string) => void;
  sendMessage: () => void;
};

export default function ChatInput({
  input,
  setInput,
  sendMessage,
}: ChatInputProps) {
  return (
    <div>
      <input
        value={input}
        onChange={(e) => setInput(e.target.value)}
        placeholder="Skriv ditt meddelande här..."
      />
      <button onClick={sendMessage}>Skicka</button>
    </div>
  );
}
