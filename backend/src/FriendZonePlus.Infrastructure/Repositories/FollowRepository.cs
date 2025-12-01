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

    public async Task<bool> ExistsAsync(int followerId, int followedUserId)
    {
        {
            return await _context.Follow
                .AnyAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);
        }
    }
    //TODO: Get followers
    //TODO: Unfollow user

}
