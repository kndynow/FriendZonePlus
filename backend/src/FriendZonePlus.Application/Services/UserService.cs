using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class UserService
{
  private readonly IUserRepository _userRepository;

  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  //Get User by Id
  public async Task<User?> GetUserByIdAsync(int id)
  {
    return await _userRepository.GetByIdAsync(id);
  }

  //Delete
  public async Task<bool> DeleteUserAsync(int id)
  {
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null) return false;

    await _userRepository.DeleteAsync(user);
    return true;
  }

}
