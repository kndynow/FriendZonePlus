import { Link } from "react-router-dom";
import UserPreview from "../components/UserPreview";

export default function HomePage() {
  return (
    <>
      <div>
        <Link to="/register">Go to register page</Link>
      </div>
      <UserPreview
        fullName="Anna Andersson"
        description="Senaste meddelandet här"
      />
      <UserPreview fullName="Anna Andersson" underButtonText="1h ago" />
      <UserPreview
        fullName="Anna Andersson"
        buttonIcon="Follow"
        onClick={() => console.log("clicked")}
      />
    </>
  );
}
