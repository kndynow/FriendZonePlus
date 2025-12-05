using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IUserRepository
{
  Task<User?> GetByIdAsync(int id);
  Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<int> ids);
  Task<User> AddAsync(User user);
  Task DeleteAsync(User user);
  Task<bool> ExistsByUsernameAsync(string username);
  Task<bool> ExistsByEmailAsync(string email);
  Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
  Task<User?> GetByUsernameAsync(string username);
  Task<User?> GetByEmailAsync(string email);
}
