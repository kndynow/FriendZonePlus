using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IFollowRepository
{
    Task AddAsync(Follow follow);
    Task<bool> ExistsAsync(int followerId, int followedUserId);
}
