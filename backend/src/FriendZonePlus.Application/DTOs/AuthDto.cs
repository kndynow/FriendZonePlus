namespace FriendZonePlus.Application.DTOs;

public record RegisterRequest(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password
    );

public record LoginRequest(
string UsernameOrEmail,
string Password
);

public record AuthResponse(
    int UserId,
    string Username
);

// Remove when registering is implemented
public record RegisterResponse(
    int UserId,
    string Username,
    DateTime CreatedAt
    );

public record LoginResponse(
    string Token,
    int UserId,
    string Username,
    string Email,
    string FirstName,
    string LastName
);


