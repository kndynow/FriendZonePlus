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

  public async Task<WallPost?> GetByIdAsync(int id)
  {
    return await _context.WallPosts.FindAsync(id);
  }

  public async Task<IEnumerable<WallPost>> GetByTargetUserIdAsync(int targetUserId)
  {
    return await _context.WallPosts.Where(p => p.TargetUserId == targetUserId)
    .OrderByDescending(p => p.CreatedAt)
    .ToListAsync();
  }

  public async Task<IEnumerable<WallPost>> GetByAuthorIdAsync(int authorId)
  {
    return await _context.WallPosts.Where(p => p.AuthorId == authorId)
    .OrderByDescending(p => p.CreatedAt)
    .ToListAsync();
  }
  public async Task<IEnumerable<WallPost>> GetFeedForUserAsync(IEnumerable<int> authorIds)
  {
    return await _context.WallPosts
        .Where(wp => authorIds.Contains(wp.AuthorId))
        .OrderByDescending(wp => wp.CreatedAt)
        .ToListAsync();
  }

  public async Task<WallPost> UpdateAsync(WallPost post)
  {
    _context.Update(post);
    await _context.SaveChangesAsync();
    return post;
  }

  public async Task DeleteAsync(int id)
  {
    var wallPost = await GetByIdAsync(id);
    if (wallPost == null)
    {
      throw new ArgumentException("Wall post does not exist");
    }
    _context.Remove(wallPost);
    await _context.SaveChangesAsync();
  }

}
