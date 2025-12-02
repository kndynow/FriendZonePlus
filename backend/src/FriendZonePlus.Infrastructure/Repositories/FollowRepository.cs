using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

  //TEST: Implementation GetFollowedUserIdsAsync
  public async Task<IReadOnlyList<int>> GetFollowedUserIdsAsync(int userId)
  {
    return await _context.Follows
      .Where(f => f.FollowerId == userId)
      .Select(f => f.FolloweeId)
      .ToListAsync();
  }
}
