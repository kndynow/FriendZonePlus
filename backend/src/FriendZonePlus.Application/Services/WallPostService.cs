using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class WallPostService
{
  private readonly IWallPostRepository _wallPostRepository;
  private readonly IUserRepository _userRepository;

  public WallPostService(IWallPostRepository wallPostRepository, IUserRepository userRepository)
  {
    _wallPostRepository = wallPostRepository;
    _userRepository = userRepository;
  }


  public async Task<WallPostResponseDto> CreateWallPostAsync(CreateWallPostDto dto)
  {
    if (string.IsNullOrWhiteSpace(dto.Content))
      throw new ArgumentException("Content cannot be empty");

    if (dto.Content.Length > 300)
      throw new ArgumentException("Content too long");

    var author = await _userRepository.GetByIdAsync(dto.AuthorId);
    if (author == null)
      throw new ArgumentException("Author does not exist");

    var target = await _userRepository.GetByIdAsync(dto.TargetUserId);
    if (target == null)
      throw new ArgumentException("Target user does not exist");

    var WallPost = new WallPost
    {
      AuthorId = dto.AuthorId,
      TargetUserId = dto.TargetUserId,
      Content = dto.Content,
      CreatedAt = DateTime.UtcNow
    };

    var createdWallPost = await _wallPostRepository.AddAsync(WallPost);

    return new WallPostResponseDto(
        createdWallPost.Id,
        createdWallPost.AuthorId,
        createdWallPost.Content,
        createdWallPost.CreatedAt
      );
  }

  public async Task<object> GetWallPostsForTargetUserAsync(int targetUserId)
  {
    throw new NotImplementedException();
  }
}
