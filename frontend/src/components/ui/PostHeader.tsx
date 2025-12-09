import UserPreview from "../../feature/user/UserPreview";
import ActionDropdown, { type ActionButton } from "./ActionDropdown";

type PostHeaderProps = {
  user: {
    fullName: string;
    imgPath?: string;
  };
  subtitle?: string;
  actions?: ActionButton[];
  userId?: number;
};

export default function PostHeader({
  user,
  subtitle,
  actions,
  userId,
}: PostHeaderProps) {
  return (
    <UserPreview
      userId={userId}
      imgPath={user.imgPath}
      fullName={user.fullName}
      subtitle={subtitle}
    >
      {actions && actions.length > 0 && <ActionDropdown actions={actions} />}
    </UserPreview>
  );
}
