using System;
using FriendZonePlus.Application.DTOs;

namespace FriendZonePlus.Application.Interfaces;

public interface IWallPostService
{
    Task<WallPostResponseDto> CreateWallPostAsync(CreateWallPostDto dto);
    Task<IEnumerable<WallPostResponseDto>> GetWallPostsForTargetUserAsync(int targetUserId);
    Task<IEnumerable<WallPostResponseDto>> GetWallPostsForAuthorAsync(int authorId);
    Task<IEnumerable<WallPostResponseDto>> GetFeedForUserAsync(int userId);
    Task<WallPostResponseDto> UpdateWallPostAsync(UpdateWallPostDto dto);
    Task<bool> DeleteWallPostAsync(int id);

}
