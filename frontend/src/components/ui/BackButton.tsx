import { useNavigate } from "react-router-dom";

export default function BackButton() {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(-1);
  };
  return (
    <button onClick={handleClick} aria-label="Go back" className="f-button">
      <i className="bi bi-arrow-left-circle" />
    </button>
  );
}
