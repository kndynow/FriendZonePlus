using System;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FriendZonePlus.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
  private readonly FriendZonePlusContext _context;

  public PostRepository(FriendZonePlusContext context)
  {
    _context = context;
  }

  public async Task<Post> AddAsync(Post post)
  {
    _context.Add(post);
    await _context.SaveChangesAsync();
    return post;
  }

  public async Task<IEnumerable<Post>> GetByTargetUserIdAsync(int targetUserId)
  {
    return await _context.Posts.Where(p => p.TargetUserId == targetUserId)
    .OrderByDescending(p => p.CreatedAt)
    .ToListAsync();
  }
}
