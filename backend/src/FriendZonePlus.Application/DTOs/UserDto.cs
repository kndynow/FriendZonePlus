namespace FriendZonePlus.Application.DTOs;

public record UserProfileDto(
    int Id,
    string Username,
    string FirstName,
    string LastName,
    string ProfilePictureUrl,
    int FollowersCount,
    int FollowingCount
);

public record UpdateUserDto(string FirstName, string LastName, string ProfilePictureUrl);

public record DeleteUserDto(int id);

public record UserResponseDto(
    string Username,
    string Email
);

public record UserListResponseDto(
    int Id,
    string Username,
    string Email
);


