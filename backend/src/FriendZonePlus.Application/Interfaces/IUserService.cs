
using FriendZonePlus.Application.DTOs;

namespace FriendZonePlus.Application.Interfaces;

public interface IUserService
{
    Task<UserProfileDto?> GetUserProfileAsync(int userId);
    Task UpdateUserProfileAsync(int userId, UpdateUserDto updateUserDto);
    Task DeleteUserAsync(int userId);

    Task FollowUserAsync(int currentUserId, int targetUserId);
    Task UnfollowUserAsync(int currentUserId, int targetUserId);
    Task<List<UserListResponseDto>> GetFollowersAsync(int userId);
    Task<List<UserListResponseDto>> GetFollowingAsync(int userId);
}
