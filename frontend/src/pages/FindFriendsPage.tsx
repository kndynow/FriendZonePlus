import { useFollowUser } from "../hooks/useFollowUser";
import { useAuth } from "../../context/AuthProvider";
import UserPreview from "../feature/user/UserPreview";
import { Col, Container, Row } from "react-bootstrap";
import { useFindFriends } from "../hooks/useFindFriends";
import toast from "react-hot-toast";

export default function FindFriendsPage() {
  const { user: currentUser } = useAuth();
  const {
    users,
    loading,
    error,
    isFollowing,
    setFollowingStatus,
    incrementFollowersCount,
    decrementFollowersCount,
  } = useFindFriends();
  const { toggleFollow, loading: followLoading } = useFollowUser();

  const handleToggleFollow = async (userId: number) => {
    const currentlyFollowing = isFollowing(userId);

    const callbacks = {
      onOptimisticUpdate: (id: number, isNowFollowing: boolean) => {
        setFollowingStatus(id, isNowFollowing);
        if (isNowFollowing) {
          incrementFollowersCount(id);
        } else {
          decrementFollowersCount(id);
        }
      },
      onRollback: (id: number, wasFollowing: boolean) => {
        setFollowingStatus(id, wasFollowing);
        if (wasFollowing) {
          incrementFollowersCount(id);
        } else {
          decrementFollowersCount(id);
        }
      },
    };

    try {
      await toggleFollow(userId, currentlyFollowing, callbacks);
    } catch (err) {
      console.error(err);
    }
  };

  if (loading) {
    return <p>Loading users...</p>;
  }

  if (error) {
    return toast.error(error as string);
  }

  return (
    <>
      {users.length === 0 ? (
        <p>No users found</p>
      ) : (
        <Row className="flex-grow-1">
          <Col>
            {users
              .filter((user) => user.id !== currentUser?.id)
              .map((user) => {
                const userIsFollowing = isFollowing(user.id);
                return (
                  <UserPreview
                    key={user.id}
                    imgPath={user.profilePictureUrl}
                    fullName={`@${user.username}`}
                    subtitle={`${user.followersCount} followers • ${user.followingCount} follows`}
                    className="f-border f-shadow semi-transparent-bg mb-2 align-items-center"
                    button={{
                      buttonIcon:
                        followLoading === user.id
                          ? "..."
                          : userIsFollowing
                          ? "–"
                          : "+",
                      onClick: () => handleToggleFollow(user.id),
                    }}
                  />
                );
              })}
          </Col>
        </Row>
      )}
    </>
  );
}
