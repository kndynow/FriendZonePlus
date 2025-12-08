using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Interfaces;

public interface IUserRepository
{

    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);

    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByIdWithRelationsAsync(int id);

    // Follow methods
    Task<bool> IsFollowingAsync(int followerId, int followedUserId);
    Task FollowUserAsync(int followerId, int followedUserId);
    Task UnfollowUserAsync(int followerId, int followedUserId);

}