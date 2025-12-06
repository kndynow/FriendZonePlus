using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
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

  public async Task<User> AddAsync(User user)
  {
    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    return user;
  }


  public async Task<User?> GetByIdAsync(int id)
  {
    return await _context.Users.FindAsync(id);
  }
  public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<int> ids)
  {
    return await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
  }

  public async Task DeleteAsync(User user)
  {
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
  }

  public async Task<bool> ExistsByUsernameAsync(string username)
  {
    return await _context.Users.AnyAsync(u => u.Username == username);
  }

  public async Task<bool> ExistsByEmailAsync(string email)
  {
    return await _context.Users.AnyAsync(u => u.Email == email);
  }

  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await _context.Users.AnyAsync(u => u.Id == id);
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
}
