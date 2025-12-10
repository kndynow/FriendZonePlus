import toast from "react-hot-toast";
import { useAuth } from "../../../context/AuthProvider";
import { useWallPost } from "../../hooks/useWallPost";
import {
  useWallPostActions,
  CREATE_POST_LOADING_ID,
} from "../../hooks/useWallPostActions";
import { useUserProfiles } from "../../hooks/useUserProfiles";
import { useFollowing } from "../../hooks/useFollowing";
import { wallPostService } from "../../api/services/wallPostService";
import WallPostItem from "./WallPostItem";
import CreateWallPostForm from "./CreateWallPostForm";
import EmptyContent from "../../components/ui/EmptyContent";

type WallPostListProps = {
  userId?: number;
  showCreateForm?: boolean;
};

export default function WallPostList({
  userId,
  showCreateForm,
}: WallPostListProps) {
  const { user: currentUser } = useAuth();
  const {
    wallPosts,
    loading,
    error,
    addWallPost,
    removeWallPost,
    updateWallPost,
  } = useWallPost(userId);
  const {
    createWallPost,
    updateWallPost: updatePost,
    deleteWallPost,
    loading: actionsLoading,
  } = useWallPostActions();
  const { isFollowing } = useFollowing();

  const authorIds = wallPosts.map((post) => post.authorId);
  const { getProfilePictureUrl } = useUserProfiles(authorIds);

  const shouldShowCreateForm =
    showCreateForm !== undefined
      ? showCreateForm
      : userId === undefined
      ? true
      : userId === currentUser?.id || isFollowing(userId);

  const targetUserId = userId ?? currentUser?.id;

  const handleCreatePost = async (content: string) => {
    if (!targetUserId || isNaN(targetUserId)) {
      toast.error("Unable to create post: Missing target user information");
      return;
    }

    const createdPost = await createWallPost({
      targetUserId,
      content,
    });
    if (createdPost) {
      addWallPost(createdPost);
    }
  };

  const handleUpdatePost = async (postId: number, newContent: string) => {
    try {
      await updatePost(postId, { content: newContent });
      // Refresh posts after successful update
      const updatedPosts = await (userId
        ? wallPostService.getWallPostsForUser(userId)
        : wallPostService.getWallPostFeed());
      const updatedPost = updatedPosts.find((p) => p.id === postId);
      if (updatedPost) {
        updateWallPost(postId, updatedPost);
      }
    } catch {
      // Error already handled by hook/interceptors
    }
  };

  const handleDeletePost = async (postId: number) => {
    await deleteWallPost(postId);
    removeWallPost(postId);
  };

  if (loading) {
    return <p>Loading posts...</p>;
  }

  if (error) {
    return <p className="text-danger">Error: {error}</p>;
  }

  return (
    <div className="w-100">
      <div className="w-100">
        {shouldShowCreateForm && (
          <CreateWallPostForm
            onSubmit={handleCreatePost}
            isLoading={actionsLoading === CREATE_POST_LOADING_ID}
          />
        )}

        {wallPosts.length === 0 ? (
          <EmptyContent
            header="No posts yet!"
            content="Maybe you should post one!"
          />
        ) : (
          <div>
            {wallPosts.map((post) => (
              <WallPostItem
                key={post.id}
                post={post}
                authorProfilePictureUrl={getProfilePictureUrl(post.authorId)}
                onUpdate={handleUpdatePost}
                onDelete={handleDeletePost}
                isUpdating={actionsLoading === post.id}
                isDeleting={actionsLoading === post.id}
              />
            ))}
          </div>
        )}
      </div>
      <br />
    </div>
  );
}
