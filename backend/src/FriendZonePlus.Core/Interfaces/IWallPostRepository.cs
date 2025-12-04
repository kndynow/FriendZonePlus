using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IWallPostRepository
{
  Task<WallPost> AddAsync(WallPost post);
  Task<WallPost?> GetByIdAsync(int id);
  Task<IEnumerable<WallPost>> GetByTargetUserIdAsync(int targetUserId);
  Task<IEnumerable<WallPost>> GetByAuthorIdAsync(int authorId);
  Task<IEnumerable<WallPost>> GetFeedForUserAsync(IEnumerable<int> userIds);
  Task<WallPost> UpdateAsync(WallPost post);
  Task DeleteAsync(int id);
}
