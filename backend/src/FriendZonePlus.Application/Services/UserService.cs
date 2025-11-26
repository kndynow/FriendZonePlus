using FriendZonePlus.Application.DTOs;
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

  public async Task<int> RegisterUserAsync(RegisterUserDto dto)
  {
    if (string.IsNullOrWhiteSpace(dto.Username))
    {
      throw new ArgumentException("Username cannot be empty");
    }

    var newUser = new User
    {
      Username = dto.Username,
      Email = dto.Email
    };

    var createdUser = await _userRepository.AddAsync(newUser);

    return createdUser.Id;
  }

}
