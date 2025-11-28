using FriendZonePlus.Core.Entities;
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

    //TODO: Follow user
    public Task AddAsync(Follows follows)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int followerId, int followeeId)
    {
        throw new NotImplementedException();
    }
    //TODO: Get followers
    //TODO: Unfollow user

}
