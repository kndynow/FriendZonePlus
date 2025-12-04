using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<User> CreateUserAsync(User user);
    Task<string?> LoginAsync(string usernameOrEmail, string password);
}