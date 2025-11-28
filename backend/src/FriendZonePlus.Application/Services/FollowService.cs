using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class FollowService
{
  private readonly IFollowRepository _followRepository;

  public FollowService(IFollowRepository followRepository)
  {
        _followRepository = followRepository;
  }

    //TODO: Get followers
    //TODO: Follow user
    //TODO: Unfollow user

}
