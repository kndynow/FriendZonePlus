using FriendZonePlus.Core.Entities;

public interface IAuthorizationService
{
    Task<User> CreateUserAsync(User user);
}