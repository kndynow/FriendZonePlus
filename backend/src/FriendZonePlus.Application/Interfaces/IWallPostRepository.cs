using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Interfaces;

public interface IWallPostRepository
{
  Task AddAsync(WallPost post);
  Task UpdateAsync(WallPost post);
  Task DeleteAsync(WallPost post);
  Task<WallPost?> GetByIdAsync(int id);
  Task<List<WallPost>> GetFeedAsync(int currentUserId);
  Task<List<WallPost>> GetWallPostsAsync(int targetUserId);
}
