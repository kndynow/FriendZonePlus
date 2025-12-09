using System;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Interfaces;
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

  public async Task AddAsync(WallPost post)
  {
    _context.WallPosts.Add(post);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(WallPost post)
  {
    _context.WallPosts.Update(post);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(WallPost post)
  {
    _context.WallPosts.Remove(post);
    await _context.SaveChangesAsync();
  }

  public async Task<WallPost?> GetByIdAsync(int id)
  {
    return await _context.WallPosts.FindAsync(id);
  }
  public async Task<List<WallPost>> GetWallPostsAsync(int targetUserId)
  {
    return await _context.WallPosts.Where(wp => wp.TargetUserId == targetUserId).Include(wp => wp.Author).OrderByDescending(wp => wp.CreatedAt).ToListAsync();
  }

  public async Task<List<WallPost>> GetFeedAsync(int currentUserId)
  {
    var followedUserId = _context.Follows.Where(f => f.FollowerId == currentUserId).Select(f => f.FollowedUserId).ToList();
    return await _context.WallPosts.Where(wp => followedUserId.Contains(wp.AuthorId)).Include(wp => wp.Author).OrderByDescending(wp => wp.CreatedAt).ToListAsync();
  }


}
