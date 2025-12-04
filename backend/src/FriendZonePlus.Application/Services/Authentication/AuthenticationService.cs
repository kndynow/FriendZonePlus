using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.Helpers;
using System.Reflection.Metadata.Ecma335;

namespace FriendZonePlus.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IJwtHelper _jwtHelper;

    public AuthenticationService(IUserRepository userRepository, IPasswordHelper passwordHelper,
    IJwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _jwtHelper = jwtHelper;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.PasswordHash = _passwordHelper.HashPassword(user.PasswordHash);

        return await _userRepository.AddAsync(user);
    }

    public async Task<string?> LoginAsync(string usernameOrEmail, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(usernameOrEmail) ??
        await _userRepository.GetByEmailAsync(usernameOrEmail);

        if (user == null || !_passwordHelper.VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }
        return _jwtHelper.GenerateToken(user);
    }
}