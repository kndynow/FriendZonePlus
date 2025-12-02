using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Helpers;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;

    public AuthorizationService(IUserRepository userRepository, IPasswordHelper passwordHelper)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
    }

    public async Task<RegisterUserResponseDto> CreateUserAsync(RegisterUserRequestDto requestDto)
    {
        var user = new User
        {
            Username = requestDto.Username,
            Email = requestDto.Email,
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            PasswordHash = _passwordHelper.HashPassword(requestDto.Password),
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