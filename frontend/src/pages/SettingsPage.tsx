import { useState } from "react";
import { Button, Col, Row } from "react-bootstrap";
import { useAuth } from "../../context/AuthProvider";
import FormField from "../components/ui/FormField";
import ProfileImage from "../components/ui/ProfileImage";

export default function SettingsPage() {
  const { user } = useAuth();

  const [firstName, setFirstName] = useState(user?.firstName ?? "");
  const [lastName, setLastName] = useState(user?.lastName ?? "");
  const [profilePictureUrl, setProfilePictureUrl] = useState(
    user?.profilePictureUrl ?? ""
  );
  const [saving, setSaving] = useState(false);

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

      alert("Ändringar sparade!");
    } catch (err) {
      alert("Något gick fel.");
    } finally {
      setSaving(false);
    }
  };
  return (
    <>
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
    </>
  );
}
