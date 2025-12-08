import { useEffect, useState } from "react";
import UserPreview from "../user/UserPreview";
import { useAuth } from "../../../context/AuthProvider";
import { Row, Col, Container } from "react-bootstrap";

export default function MessagesPage() {
  const { user } = useAuth();
  const [followers, setFollowers] = useState([]);
  const [latestMessages, setLatestMessages] = useState<{
    [key: string]: string;
  }>({});

  // Fetch followers
  useEffect(() => {
    async function fetchFollowers() {
      if (!user) return;

      try {
        const res = await fetch(`/api/users/me/following`);
        const data = await res.json();
        console.log(data);
        setFollowers(data);
      } catch (error) {
        console.error("Failed to fetch followers:", error);
      }
    }

    fetchFollowers();
  }, [user]);

  // Fetch latest messages
  useEffect(() => {
    async function fetchLatestMessages() {
      if (!user) return;

      try {
        const res = await fetch(`/api/Message/latest/`);
        const data = await res.json();
        const messagesMap: { [key: string]: string } = {};
        data.forEach((msg: any) => {
          messagesMap[msg.senderId] = msg.message;
        });
        setLatestMessages(messagesMap);
      } catch (error) {
        console.error("Failed to fetch latest messages:", error);
      }
    }

    fetchLatestMessages();
  }, [user]);

  return (
    <>
      <Container>
        {followers.map((follower: any) => (
          <Row
            key={follower.id}
            className="f-border f-shadow semi-transparent-bg pt-2 mb-2"
          >
            <Col>
              <div>
                <UserPreview
                  fullName={`${follower.firstName} ${follower.lastName}`}
                  imgPath={follower.imgPath}
                  subtitle={latestMessages[follower.id] || "No messages yet"}
                />
              </div>
            </Col>
          </Row>
        ))}
      </Container>
    </>
  );
}
