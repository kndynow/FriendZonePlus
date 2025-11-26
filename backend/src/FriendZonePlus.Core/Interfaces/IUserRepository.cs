using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IUserRepository
{
  Task<User> AddAsync(User user);
}
