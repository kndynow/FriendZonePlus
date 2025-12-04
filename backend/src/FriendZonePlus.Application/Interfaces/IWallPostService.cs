using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Interfaces;

public interface IWallPostService
{
    Task<WallPost> CreateWallPostAsync(WallPost wallPost);
    Task<IEnumerable<WallPost>> GetWallPostsForTargetUserAsync(int targetUserId);
    Task<IEnumerable<WallPost>> GetWallPostsForAuthorAsync(int authorId);
    Task<IEnumerable<WallPost>> GetFeedForUserAsync(int userId);
    Task<WallPost> UpdateWallPostAsync(WallPost wallPost);
    Task<bool> DeleteWallPostAsync(int id);

}
