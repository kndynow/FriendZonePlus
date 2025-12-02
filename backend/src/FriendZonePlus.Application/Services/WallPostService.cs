using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class WallPostService : IWallPostService
{
  private readonly IWallPostRepository _wallPostRepository;
  private readonly IUserRepository _userRepository;
  private readonly IFollowRepository _followRepository;
  public WallPostService(IWallPostRepository wallPostRepository, IUserRepository userRepository, IFollowRepository followRepository)
  {
    _wallPostRepository = wallPostRepository;
    _userRepository = userRepository;
    _followRepository = followRepository;
  }


  public async Task<WallPostResponseDto> CreateWallPostAsync(CreateWallPostDto dto)
  {
    //Validate content is not empty
    if (string.IsNullOrWhiteSpace(dto.Content))
      throw new ArgumentException("Content cannot be empty");

    //Validate content is not too long
    if (dto.Content.Length > 300)
      throw new ArgumentException("Content too long");

    //Validate author exists
    var author = await _userRepository.GetByIdAsync(dto.AuthorId);
    if (author == null)
      throw new ArgumentException("Author does not exist");

    //Validate target user exists
    var target = await _userRepository.GetByIdAsync(dto.TargetUserId);
    if (target == null)
      throw new ArgumentException("Target user does not exist");

    //Create post
    var WallPost = new WallPost
    {
      AuthorId = dto.AuthorId,
      TargetUserId = dto.TargetUserId,
      Content = dto.Content,
      CreatedAt = DateTime.UtcNow
    };

    //Add post to database
    var createdWallPost = await _wallPostRepository.AddAsync(WallPost);

    //Return post
    return new WallPostResponseDto(
        createdWallPost.Id,
        createdWallPost.AuthorId,
        createdWallPost.Content,
        createdWallPost.CreatedAt
      );
  }

  public async Task<IEnumerable<WallPostResponseDto>> GetFeedForUserAsync(int userId)
  {
    //Validate user exists
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null)
    {
      throw new ArgumentException("User does not exist");
    }

    // Get IDs for all users the current user follows
    var followedUserIds = await _followRepository.GetFollowedUserIdsAsync(userId);

    // If current user don't follow any users return empty list
    if (!followedUserIds.Any())
    {
      return Enumerable.Empty<WallPostResponseDto>();
    }

    // Get all posts from followed users
    var wallPosts = await _wallPostRepository.GetFeedForUserAsync(followedUserIds);

    return wallPosts.Select(wp => new WallPostResponseDto(
      wp.Id,
      wp.AuthorId,
      wp.Content,
      wp.CreatedAt
    ));
  }

  public async Task<IEnumerable<WallPostResponseDto>> GetWallPostsForAuthorAsync(int authorId)
  {
    //Validate author exists
    var author = await _userRepository.GetByIdAsync(authorId);
    if (author == null)
    {
      throw new ArgumentException("Author does not exist");
    }

    // Get all posts for the author
    var wallPosts = await _wallPostRepository.GetByAuthorIdAsync(authorId);
    return wallPosts.Select(wp => new WallPostResponseDto(wp.Id, wp.AuthorId, wp.Content, wp.CreatedAt));
  }
  public async Task<IEnumerable<WallPostResponseDto>> GetWallPostsForTargetUserAsync(int targetUserId)
  {
    //Validate target user exists
    var target = await _userRepository.GetByIdAsync(targetUserId);
    if (target == null)
    {
      throw new ArgumentException("Target user does not exist");
    }

    // Get all posts for the target user
    var wallPosts = await _wallPostRepository.GetByTargetUserIdAsync(targetUserId);
    return wallPosts.Select(wp => new WallPostResponseDto(wp.Id, wp.AuthorId, wp.Content, wp.CreatedAt));
  }

  public async Task<WallPostResponseDto> UpdateWallPostAsync(UpdateWallPostDto dto)
  {
    //Validate post exists
    var wallPost = await _wallPostRepository.GetByIdAsync(dto.Id);
    if (wallPost == null)
    {
      throw new ArgumentException("Post does not exist");
    }

    //Update post
    wallPost.Content = dto.Content;
    wallPost.UpdatedAt = DateTime.UtcNow;
    await _wallPostRepository.UpdateAsync(wallPost);

    return new WallPostResponseDto(wallPost.Id, wallPost.AuthorId, wallPost.Content, wallPost.CreatedAt);
  }

  public async Task DeleteWallPostAsync(int id)
  {
    //Validate post exists
    var wallPost = await _wallPostRepository.GetByIdAsync(id);
    if (wallPost == null)
    {
      throw new ArgumentException("Post does not exist");
    }
    //Delete post
    await _wallPostRepository.DeleteAsync(id);
  }

}
