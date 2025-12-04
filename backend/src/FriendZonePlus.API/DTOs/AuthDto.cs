namespace FriendZonePlus.API.DTOs;

public record RegisterUserRequestDto(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password
    );

public record RegisterUserResponseDto(
    int Id,
    string Username,
    DateTime CreatedAt
    );

public record LoginRequestDto(
    string UsernameOrEmail,
    string Password
);

public record LoginResponseDto(
    string Token,
    int UserId,
    string Username
);