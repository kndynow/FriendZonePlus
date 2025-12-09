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
    decrementFollowersCount
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
    return (
      <p>Loading users...</p>
    );
  }

  if (error) {
    return (
      toast.error(error as string)
    );
  }

  return (
    <Container className="py-4">
      {users.length === 0 ? (
        <p>No users found</p>
      ) : (
        <Container>
          <Row>
            <Col >
              {users
                .filter(user => user.id !== currentUser?.id)
                .map((user) => {
                  const userIsFollowing = isFollowing(user.id);
                  return (
                    <UserPreview
                      key={user.id}
                      imgPath={user.profilePictureUrl}
                      fullName={`${user.firstName} ${user.lastName}`}
                      subtitle={`@${user.username} • ${user.followersCount} followers`}
                      className="f-border f-shadow semi-transparent-bg pt-2 mb-2 gap-2"
                      button={{
                        buttonIcon: followLoading === user.id ? "..." : userIsFollowing ? "Unfollow" : "Follow",
                        onClick: () => handleToggleFollow(user.id),
                      }}
                    />
                  );
                })}
            </Col>
          </Row>
        </Container>
      )}
    </Container>
  );
}