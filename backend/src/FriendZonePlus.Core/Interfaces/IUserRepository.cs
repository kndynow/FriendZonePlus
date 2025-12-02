using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IUserRepository
{
  Task<User?> GetByIdAsync(int id);
  Task<User> AddAsync(User user);
  Task DeleteAsync(User user);
  Task<bool> ExistsByUsernameAsync(string username);
  Task<bool> ExistsByEmailAsync(string email);
}
