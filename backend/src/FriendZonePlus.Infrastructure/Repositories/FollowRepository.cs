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

  // Follow user
  public async Task<Follow> AddAsync(Follow follow)
  {
    _context.Follows.Add(follow);
    await _context.SaveChangesAsync();
    return follow;
  }

  // Check if follow relationship exists
  public async Task<bool> ExistsAsync(int followerId, int followedUserId)
  {
    return await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);
  }

  // Unfollow user
  public async Task RemoveAsync(int followerId, int followedUserId)
  {

    var rowsAffected = await _context.Follows
      .Where(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId)
      .ExecuteDeleteAsync();

  }

  // Get list of user ids that follow the user
  public async Task<IReadOnlyList<int>> GetFollowerIdsAsync(int userId)
  {
    return await _context.Follows
    .Where(f => f.FollowedUserId == userId)
    .Select(f => f.FollowerId)
    .ToListAsync();
  }

  //Get list of user ids that the user follows
  public async Task<IReadOnlyList<int>> GetFollowedUserIdsAsync(int userId)
  {
    return await _context.Follows
      .Where(f => f.FollowerId == userId)
      .Select(f => f.FollowedUserId)
      .ToListAsync();
  }

}
