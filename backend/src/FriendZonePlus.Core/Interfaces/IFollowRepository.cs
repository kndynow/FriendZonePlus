using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IFollowRepository
{
    Task AddAsync(Follow follow);
    Task DeleteAsync(Follow follow);
    Task<bool> ExistsAsync(int followerId, int followedUserId);
    Task<Follow?> GetFollowRelationAsync(int followerId, int followedUserId);
}
