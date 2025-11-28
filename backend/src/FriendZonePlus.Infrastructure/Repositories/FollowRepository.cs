using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Data;

namespace FriendZonePlus.Infrastructure.Repositories;

public class FollowRepository : IFollowRepository
{
  private readonly FriendZonePlusContext _context;

  public FollowRepository(FriendZonePlusContext context)
  {
    _context = context;
  }
    //TODO: Get followers
    //TODO: Follow user
    //TODO: Unfollow user

}
