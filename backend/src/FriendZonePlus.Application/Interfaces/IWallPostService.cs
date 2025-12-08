using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Interfaces;

public interface IWallPostService
{
    Task<WallPostResponseDto> CreateAsync(int currentUserId, CreateWallPostDto dto);
    Task UpdateWallPostAsync(int currentUserId, int wallPostId, UpdateWallPostDto dto);
    Task DeleteWallPostAsync(int currentUserId, int wallPostId);
    Task<List<WallPostResponseDto>> GetFeedAsync(int currentUserId);
    Task<List<WallPostResponseDto>> GetWallPostsAsync(int userId);

}
