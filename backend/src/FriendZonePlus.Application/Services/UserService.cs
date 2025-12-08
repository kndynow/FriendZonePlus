using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Exceptions;
using Mapster;

namespace FriendZonePlus.Application.Services;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;

  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
  {
    var user = await _userRepository.GetByIdWithRelationsAsync(userId);
    if (user is null)
    {
      throw new UserNotFoundException($"User with ID {userId} not found");
    }

    return user.Adapt<UserProfileDto>();
  }
  public async Task UpdateUserProfileAsync(int userId, UpdateUserDto updateUserDto)
  {
    var user = await _userRepository.GetByIdAsync(userId);
    if (user is null) throw new UserNotFoundException($"User with ID {userId} not found");

    user.FirstName = updateUserDto.FirstName;
    user.LastName = updateUserDto.LastName;
    user.ProfilePictureUrl = updateUserDto.ProfilePictureUrl;

    await _userRepository.UpdateAsync(user);
  }

  public async Task DeleteUserAsync(int userId)
  {
    var user = await _userRepository.GetByIdAsync(userId);
    if (user is null) throw new UserNotFoundException($"User with ID {userId} not found");

    await _userRepository.DeleteAsync(user);
  }

  public async Task FollowUserAsync(int currentUserId, int targetUserId)
  {
    if (currentUserId == targetUserId)
      throw new CannotFollowSelfException();

    var targetUser = await _userRepository.GetByIdAsync(targetUserId);
    if (targetUser is null)
      throw new UserNotFoundException($"User with ID {targetUserId} not found");

    await _userRepository.FollowUserAsync(currentUserId, targetUserId);
  }

  public async Task UnfollowUserAsync(int currentUserId, int targetUserId)
  {
    var targetUser = await _userRepository.GetByIdAsync(targetUserId);
    if (targetUser is null)
      throw new UserNotFoundException($"User with ID {targetUserId} not found");

    await _userRepository.UnfollowUserAsync(currentUserId, targetUserId);
  }

}
