import { Button, Col, Row } from "react-bootstrap";
import { useAuth } from "../../../context/AuthProvider";
import ProfileImage from "../../components/ui/ProfileImage";
import FormField from "../../components/ui/FormField";
import { useUserProfile } from "../../hooks/useUserProfile";

export default function SettingsPage() {
  const { user } = useAuth();

  const { profile, setProfile, loading, updateProfile } = useUserProfile(
    user?.id ?? null
  );

  const handleChange = (field: string, value: string) => {
    setProfile((prev) => ({ ...prev, [field]: value }));
  };

  return (
    <Row className="flex-grow-1 mb-5 pb-5">
      <Col>
        <div className="d-flex flex-column align-items-center mb-1">
          <ProfileImage
            imgPath={profile.profilePictureUrl}
            className="profile-img-large"
          />
        </div>

        <FormField
          label="Profile picture URL"
          placeholder="Enter image URL"
          value={profile.profilePictureUrl}
          onChange={(e) => handleChange("profilePictureUrl", e.target.value)}
        />

        <FormField
          label="First name"
          placeholder="Enter first name"
          value={profile.firstName}
          onChange={(e) => handleChange("firstName", e.target.value)}
        />

        <FormField
          label="Last name"
          placeholder="Enter last name"
          value={profile.lastName}
          onChange={(e) => handleChange("lastName", e.target.value)}
        />

        <Button
          className="w-100 mt-3 mb-4"
          onClick={updateProfile}
          disabled={loading}
        >
          Save changes
        </Button>
      </Col>
    </Row>
  );
}
