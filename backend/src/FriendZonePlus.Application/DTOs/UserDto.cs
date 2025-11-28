namespace FriendZonePlus.Application.DTOs;

public record CreateUserDto(string Username, string Email);

public record UserResponseDto(string Username, string Email);

public record DeleteUserDto(int id);

