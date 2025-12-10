import { useEffect, useState } from "react";
import UserPreview from "../user/UserPreview";
import { useAuth } from "../../../context/AuthProvider";
import { Row, Col, Stack, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import type { Follower } from "../../../types/followers";
import EmptyContent from "../../components/ui/EmptyContent";

export default function MessagesPage() {
  const navigate = useNavigate();
  const { user } = useAuth();
  const [loading, setLoading] = useState(true);
  const [followers, setFollowers] = useState<Follower[]>([]);
  const [latestMessages, setLatestMessages] = useState<{
    [key: string]: { senderName: string; content: string };
  }>({});

  useEffect(() => {
    async function fetchAll() {
      if (!user) return;

      try {
        const [followersRes, messagesRes] = await Promise.all([
          fetch(`/api/users/me/following`),
          fetch(`/api/Message/latest/`),
        ]);

        if (!followersRes.ok || !messagesRes.ok) {
          throw new Error("Failed to load");
        }

        const followersData = await followersRes.json();
        const messagesData = await messagesRes.json();

        setFollowers(followersData);

        const messagesMap: any = {};
        messagesData.forEach((msg: any) => {
          const otherUserId =
            msg.senderId === user.id ? msg.receiverId : msg.senderId;

          const sender =
            msg.senderId === user.id
              ? "You"
              : followersData.find((f: any) => f.id === msg.senderId)
                  ?.firstName || "Unknown";

          messagesMap[otherUserId] = {
            senderName: sender,
            content: msg.content,
          };
        });

        setLatestMessages(messagesMap);
      } catch (error) {
      } finally {
        setLoading(false);
      }
    }

    fetchAll();
  }, [user]);

  if (loading) {
    return (
      <Spinner animation="border" role="status">
        <span className="visually-hidden">Loading...</span>
      </Spinner>
    );
  }

  if (followers.length === 0) {
    return (
      <EmptyContent
        header="AHHHH! No friends yet?"
        content="Follow someone to send them message!"
      />
    );
  }

  return (
    <>
      <Row className="flex-grow-1">
        <Col>
          {followers.map((follower) => (
            <div onClick={() => navigate(`/messages/${follower.id}`)}>
              <UserPreview
                key={follower.id}
                fullName={`${follower.firstName} ${follower.lastName}`}
                imgPath={`${follower.profilePictureUrl}`}
                subtitle={
                  latestMessages[follower.id]
                    ? `${latestMessages[follower.id].senderName}: ${
                        latestMessages[follower.id].content
                      }`
                    : "Start a conversation..."
                }
                className="f-border f-shadow semi-transparent-bg mb-2 align-items-center p-3"
                truncate={true}
                truncateLength={31}
              />
            </div>
          ))}
        </Col>
      </Row>
      <br />
    </>
  );
}
