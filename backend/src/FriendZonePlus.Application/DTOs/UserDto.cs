namespace FriendZonePlus.Application.DTOs;

public record UserResponseDto(
    string Username,
    string Email
);

public record DeleteUserDto(int id);

