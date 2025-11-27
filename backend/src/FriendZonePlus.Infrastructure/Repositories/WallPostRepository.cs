using System;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FriendZonePlus.Infrastructure.Repositories;

public class WallPostRepository : IWallPostRepository
{
  private readonly FriendZonePlusContext _context;

  public WallPostRepository(FriendZonePlusContext context)
  {
    _context = context;
  }

  public async Task<WallPost> AddAsync(WallPost wallPost)
  {
    _context.Add(wallPost);
    await _context.SaveChangesAsync();
    return wallPost;
  }

  public async Task<IEnumerable<WallPost>> GetByTargetUserIdAsync(int targetUserId)
  {
    return await _context.WallPosts.Where(p => p.TargetUserId == targetUserId)
    .OrderByDescending(p => p.CreatedAt)
    .ToListAsync();
  }
}
