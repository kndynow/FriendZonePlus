import { useFollowUser } from "../hooks/useFollowUser";
import { useAuth } from "../../context/AuthProvider";
import UserPreview from "../feature/user/UserPreview";
import { Col, Row } from "react-bootstrap";
import { useFindFriends } from "../hooks/useFindFriends";
import toast from "react-hot-toast";
import EmptyContent from "../components/ui/EmptyContent";

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

  if (users.length === 1) {
    return (
      <EmptyContent
        header="No other users yet"
        content="Hang along and maybe they'll come!"
      />
    );
  }

  return (
    <>
      <Row className="flex-grow-1">
        <Col>
          {users
            .filter((user) => user.id !== currentUser?.id)
            .map((user) => {
              const userIsFollowing = isFollowing(user.id);
              return (
                <UserPreview
                  key={user.id}
                  userId={user.id}
                  imgPath={user.profilePictureUrl}
                  fullName={`@${user.username}`}
                  subtitle={`${user.followersCount} followers • follows ${user.followingCount} `}
                  className="f-border f-shadow semi-transparent-bg mb-2 align-items-center ps-3"
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
    </>
  );
}
