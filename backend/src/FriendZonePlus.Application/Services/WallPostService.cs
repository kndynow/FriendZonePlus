using System;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.DTOs;
using Mapster;
using FriendZonePlus.Core.Exceptions;

namespace FriendZonePlus.Application.Services;

public class WallPostService : IWallPostService
{
  private readonly IWallPostRepository _wallPostRepository;
  private readonly IUserRepository _userRepository;
  public WallPostService(IWallPostRepository wallPostRepository, IUserRepository userRepository)
  {
    _wallPostRepository = wallPostRepository;
    _userRepository = userRepository;
  }

  public async Task<WallPostResponseDto> CreateAsync(int currentUserId, CreateWallPostDto dto)
  {
    var post = dto.Adapt<WallPost>();
    post.AuthorId = currentUserId;

    await _wallPostRepository.AddAsync(post);

    // Hämta posten igen med Author inkluderad för korrekt mappning
    var createdPost = await _wallPostRepository.GetByIdAsync(post.Id);
    if (createdPost is null)
    {
      throw new InvalidOperationException("Failed to retrieve created post");
    }

    return createdPost.Adapt<WallPostResponseDto>();
  }
  public async Task UpdateWallPostAsync(int currentUserId, int wallPostId, UpdateWallPostDto dto)
  {
    var post = await _wallPostRepository.GetByIdAsync(wallPostId);
    if (post is null) throw new PostNotFoundException(wallPostId);

    if (post.AuthorId != currentUserId) throw new UnauthorizedPostAccessException();

    post.Content = dto.Content;
    await _wallPostRepository.UpdateAsync(post);
  }

  public async Task DeleteWallPostAsync(int currentUserId, int wallPostId)
  {
    var post = await _wallPostRepository.GetByIdAsync(wallPostId);
    if (post is null) throw new PostNotFoundException(wallPostId);

    if (post.AuthorId != currentUserId && post.TargetUserId != currentUserId) throw new UnauthorizedPostAccessException();

    await _wallPostRepository.DeleteAsync(post);
  }

  public async Task<List<WallPostResponseDto>> GetFeedAsync(int currentUserId)
  {
    var posts = await _wallPostRepository.GetFeedAsync(currentUserId);
    return posts.Adapt<List<WallPostResponseDto>>();
  }

  public async Task<List<WallPostResponseDto>> GetWallPostsAsync(int userId)
  {
    var posts = await _wallPostRepository.GetWallPostsAsync(userId);
    return posts.Adapt<List<WallPostResponseDto>>();
  }

}
