import ProfileImage from "../../components/ui/ProfileImage";

type SingleMessageProps = {
  from: string;
  content: string;
  imgPath?: string;
};

export default function SingleMessage({
  from,
  content,
  imgPath,
}: SingleMessageProps) {
  const isFromMe = from === "me";
  const showProfileImage = !isFromMe;

  return (
    <div className={`message-wrapper ${isFromMe ? "me" : "other"}`}>
      {showProfileImage && <ProfileImage imgPath={imgPath} />}
      <div className={`message f-border ${isFromMe ? "me" : "other"}`}>
        <span>{content}</span>
      </div>
    </div>
  );
}
