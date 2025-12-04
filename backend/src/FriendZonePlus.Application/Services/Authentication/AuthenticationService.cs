using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Helpers.PasswordHelpers;

namespace FriendZonePlus.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;

    public AuthenticationService(IUserRepository userRepository, IPasswordHelper passwordHelper)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.PasswordHash = _passwordHelper.HashPassword(user.PasswordHash);

        return await _userRepository.AddAsync(user);
    }
}