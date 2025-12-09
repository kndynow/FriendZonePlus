type ProfileImageProps = {
  imgPath?: string;
  className?: string;
};

export default function ProfileImage({
  imgPath,
  className,
}: ProfileImageProps) {
  const imageSrc = imgPath || "/images/profilePlaceholder.png";
  const altText = imgPath ? "User profile image" : "Profile placeholder";

  return (
    <img
      src={imageSrc}
      alt={altText}
      className={`profile-img f-shadow ${className}`}
    />
  );
}
