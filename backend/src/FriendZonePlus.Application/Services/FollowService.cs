using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class FollowService
{
  private readonly IFollowRepository _followRepository;

  public FollowService(IFollowRepository followRepository)
  {
        _followRepository = followRepository;
  }

    //TODO: Follow user
    public async Task FollowAsync(int followerId, int followeeId)
    {
        if (followerId == followeeId)
        {
            throw new InvalidOperationException("User cannot follow themselves.");
        }
        if (await _followRepository.ExistsAsync(followerId, followeeId))
        {
            throw new InvalidOperationException("Follow relationship already exists.");
        }

        var follow = new Follows
        {
            FollowerId = followerId,
            FolloweeId = followeeId
        };

        await _followRepository.AddAsync(follow);
    }

    //TODO: Get followers
    //TODO: Unfollow user

}
