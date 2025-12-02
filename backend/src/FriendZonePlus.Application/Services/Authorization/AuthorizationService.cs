using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.DTOs;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;

    public AuthorizationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegisterUserResponseDto> CreateUserAsync(RegisterUserRequestDto requestDto)
    {
        var user = new User
        {
            Username = requestDto.Username,
            Email = requestDto.Email,
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            PasswordHash = requestDto.Password,
            CreatedAt = DateTime.UtcNow
        };

        var savedUser = await _userRepository.AddAsync(user);

        return new RegisterUserResponseDto(
            savedUser.Id,
            savedUser.Username,
            savedUser.CreatedAt
        );
    }
}