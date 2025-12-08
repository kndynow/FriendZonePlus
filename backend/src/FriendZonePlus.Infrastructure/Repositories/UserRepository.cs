using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FriendZonePlus.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
  private readonly FriendZonePlusContext _context;

  public UserRepository(FriendZonePlusContext context)
  {
    _context = context;
  }

  public async Task AddAsync(User user)
  {
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(User user)
  {
    _context.Users.Update(user);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(User user)
  {
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
  }

  public async Task<User?> GetByIdAsync(int id)
  {
    return await _context.Users.FindAsync(id);
  }

  public async Task<User?> GetByUsernameAsync(string username)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
  }

  public async Task<User?> GetByEmailAsync(string email)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
  }

  public async Task<User?> GetByIdWithRelationsAsync(int id)
  {
    return await _context.Users.Include(u => u.Followers)
      .Include(u => u.Following)
      .FirstOrDefaultAsync(u => u.Id == id);
  }

  public async Task<bool> IsFollowingAsync(int followerId, int followedUserId)
  {
    return await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);
  }

  public async Task FollowUserAsync(int followerId, int followedUserId)
  {
    var follow = new Follow { FollowerId = followerId, FollowedUserId = followedUserId };
    await _context.Follows.AddAsync(follow);
    await _context.SaveChangesAsync();
  }

  public async Task UnfollowUserAsync(int followerId, int followedUserId)
  {
    var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);

    if (follow != null)
    {
      _context.Follows.Remove(follow);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<List<User>> GetFollowersAsync(int userId)
  {
    return await _context.Follows
      .Where(f => f.FollowedUserId == userId)
      .Include(f => f.Follower)
      .Select(f => f.Follower)
      .ToListAsync();
  }

  public async Task<List<User>> GetFollowingAsync(int userId)
  {
    return await _context.Follows
      .Where(f => f.FollowerId == userId)
      .Include(f => f.FollowedUser)
      .Select(f => f.FollowedUser)
      .ToListAsync();
  }

  public async Task<List<User>> GetAllUsersAsync()
  {
    return await _context.Users
      .Include(u => u.Followers)
      .Include(u => u.Following)
      .ToListAsync();
  }
}
