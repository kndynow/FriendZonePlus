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

    public async Task AddAsync(Follow follow)
    {
        _context.Follow.Add(follow);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Follow follow)
    {
        _context.Follow.Remove(follow);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int followerId, int followedUserId)
    {
        {
            return await _context.Follow
                .AnyAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);
        }
    }

    public async Task<Follow?> GetFollowRelationAsync(int followerId, int followedUserId)
    {
        return await _context.Follow
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);
    }
    //TODO: Get followers
    //TODO: Get followed users
}
