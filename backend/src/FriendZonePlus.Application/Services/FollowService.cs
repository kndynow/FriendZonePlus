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
