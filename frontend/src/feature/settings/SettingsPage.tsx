import { useEffect, useState } from "react";
import { Button, Col, Row } from "react-bootstrap";
import { useAuth } from "../../../context/AuthProvider";
import ProfileImage from "../../components/ui/ProfileImage";
import FormField from "../../components/ui/FormField";
import { useUsers } from "../../hooks/useUsers";
import toast from "react-hot-toast";

export default function SettingsPage() {
  const { user } = useAuth();
  const { fetchUserProfile } = useUsers();

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [profilePictureUrl, setProfilePictureUrl] = useState("");
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!user?.id) return;

    fetchUserProfile(user.id).then((data) => {
      if (!data) return;
      setFirstName(data.firstName);
      setLastName(data.lastName);
      setProfilePictureUrl(data.profilePictureUrl);
    });
  }, [user]);

  const handleSave = async () => {
    setSaving(true);

    try {
      await fetch("/api/users/me", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({
          firstName,
          lastName,
          profilePictureUrl,
        }),
      });
      toast.success("Changes has been saved");
    } catch {
      toast.error("Failed to save changes, please try again later.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Row className="flex-grow-1 mb-5 pb-5">
      <Col>
        <div className="d-flex flex-column align-items-center mb-1">
          <ProfileImage
            imgPath={profilePictureUrl}
            className="profile-img-large"
          />
        </div>

        <FormField
          label="Profile picture URL"
          placeholder="Enter image URL"
          value={profilePictureUrl}
          onChange={(e) => setProfilePictureUrl(e.target.value)}
        />

        <FormField
          label="First name"
          placeholder="Enter first name"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
        />

        <FormField
          label="Last name"
          placeholder="Enter last name"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
        />

        <Button
          className="w-100 mt-3 mb-4"
          onClick={handleSave}
          disabled={saving}
        >
          {saving ? "Saving..." : "Save changes"}
        </Button>
      </Col>
    </Row>
  );
}
