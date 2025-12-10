import { useParams } from "react-router-dom";
import { useAuth } from "../../context/AuthProvider";
import WallPostList from "../feature/wallpost/WallPostList";

export default function UserPage() {
  const { id } = useParams();
  const { user: currentUser } = useAuth();
  const parsedId = id ? parseInt(id, 10) : NaN;
  const userId = !isNaN(parsedId) ? parsedId : currentUser?.id;

  return (
    <>
      <WallPostList userId={userId} showCreateForm={true} />
    </>
  );
}
