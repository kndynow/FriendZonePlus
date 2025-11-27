using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IPostRepository
{
  Task<Post> AddAsync(Post post);
  Task<IEnumerable<Post>> GetByTargetUserIdAsync(int targetUserId);
}
