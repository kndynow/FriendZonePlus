import { Button, Col, Row } from "react-bootstrap";
import { useAuth } from "../../../context/AuthProvider";
import ProfileImage from "../../components/ui/ProfileImage";
import FormField from "../../components/ui/FormField";
import { useUserProfile } from "../../hooks/useUserProfile";

export default function SettingsPage() {
  const { user, logout } = useAuth();
  const { profile, setProfile, loading, updateProfile } = useUserProfile(
    user?.id ?? null
  );

  const handleChange = (field: string, value: string) =>
    setProfile((prev) => ({ ...prev, [field]: value }));

  const fields = [
    {
      name: "profilePictureUrl",
      label: "Profile picture",
      placeholder: "Enter image URL",
    },
    {
      name: "firstName",
      label: "First name",
      placeholder: "Enter first name",
    },
    {
      name: "lastName",
      label: "Last name",
      placeholder: "Enter last name",
    },
  ] as const;

  return (
    <Row className="flex-grow-1 mb-5 pb-5">
      <Col>
        <div className="d-flex flex-column align-items-center mb-1">
          <ProfileImage
            imgPath={profile.profilePictureUrl}
            className="profile-img-large"
          />
        </div>

        {fields.map((field) => (
          <FormField
            key={field.name}
            label={field.label}
            placeholder={field.placeholder}
            value={profile[field.name]}
            onChange={(e) => handleChange(field.name, e.target.value)}
          />
        ))}

        <Button
          className="w-100 my-1"
          onClick={updateProfile}
          disabled={loading}
        >
          {loading ? "Saving..." : "Save changes"}
        </Button>

        <Button
          className="w-100 mt-3"
          variant="danger"
          onClick={logout}
          disabled={loading}
        >
          Sign out
        </Button>
      </Col>
    </Row>
  );
}
