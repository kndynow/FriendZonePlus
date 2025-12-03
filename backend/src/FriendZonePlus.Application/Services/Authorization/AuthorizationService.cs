using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.DTOs;


public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;

    public AuthorizationService(IUserRepository userRepository, IPasswordHelper passwordHelper)
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