namespace FriendZonePlus.Application.Services
{
    public interface IFollowService
    {
        Task FollowAsync(int followerId, int followedUserId);
        Task UnfollowAsync(int followerId, int followedUserId);
    }
}