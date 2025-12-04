using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IFollowRepository
{
    // Add follow relationship aka follow user
    Task<Follow> AddAsync(Follow follow);

    // Remove follow relationship aka unfollow user
    Task RemoveAsync(int followerId, int followedUserId);

    // Check if follow relationship exists
    Task<bool> ExistsAsync(int followerId, int followedUserId);

    // Get followers
    Task<IReadOnlyList<int>> GetFollowerIdsAsync(int userId);

    // Get followed user IDs
    Task<IReadOnlyList<int>> GetFollowedUserIdsAsync(int userId);

}
