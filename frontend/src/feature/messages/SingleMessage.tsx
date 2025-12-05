type SingleMessageProps = {
  from: string;
  content: string;
};
export default function SingleMessage({ from, content }: SingleMessageProps) {
  return (
    <div className={`message f-border ${from === "me" ? "me" : "other"}`}>
      <span>{content}</span>
    </div>
  );
}
