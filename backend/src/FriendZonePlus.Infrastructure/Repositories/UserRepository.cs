using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Data;

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
}
