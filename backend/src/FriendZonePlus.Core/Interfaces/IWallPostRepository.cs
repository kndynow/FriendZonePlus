using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IWallPostRepository
{
  Task<WallPost> AddAsync(WallPost post);
  Task<IEnumerable<WallPost>> GetByTargetUserIdAsync(int targetUserId);
}
