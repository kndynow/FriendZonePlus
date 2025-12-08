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
    return post.Adapt<WallPostResponseDto>();
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




  // public async Task<WallPost> CreateWallPostAsync(WallPost wallPost)
  // {
  //   // Validate content
  //   if (string.IsNullOrWhiteSpace(wallPost.Content))
  //   {
  //     throw new ArgumentException("Content cannot be empty");
  //   }

  //   if (wallPost.Content.Length > 300)
  //   {
  //     throw new ArgumentException("Content too long");
  //   }

  //   //Validate author exists
  //   var author = await _userRepository.GetByIdAsync(wallPost.AuthorId);
  //   if (author == null)
  //     throw new ArgumentException("Author does not exist");

  //   //Validate target user exists
  //   var target = await _userRepository.GetByIdAsync(wallPost.TargetUserId);
  //   if (target == null)
  //     throw new ArgumentException("Target user does not exist");

  //   //Add post to database
  //   return await _wallPostRepository.AddAsync(wallPost);
  // }

  // public async Task<IEnumerable<WallPost>> GetFeedForUserAsync(int userId)
  // {
  //   //Validate user exists
  //   var user = await _userRepository.GetByIdAsync(userId);
  //   if (user == null)
  //   {
  //     throw new ArgumentException("User does not exist");
  //   }

  //   // Get IDs for all users the current user follows
  //   var followedUserIds = await _followRepository.GetFollowedUserIdsAsync(userId);


  //   return await _wallPostRepository.GetFeedForUserAsync(followedUserIds);
  // }

  // public async Task<IEnumerable<WallPost>> GetWallPostsForAuthorAsync(int authorId)
  // {
  //   //Validate author exists
  //   var author = await _userRepository.GetByIdAsync(authorId);
  //   if (author == null)
  //   {
  //     throw new ArgumentException("Author does not exist");
  //   }

  //   // Get all posts for the author
  //   return await _wallPostRepository.GetByAuthorIdAsync(authorId);
  // }
  // public async Task<IEnumerable<WallPost>> GetWallPostsForTargetUserAsync(int targetUserId)
  // {
  //   //Validate target user exists
  //   var target = await _userRepository.GetByIdAsync(targetUserId);
  //   if (target == null)
  //   {
  //     throw new ArgumentException("Target user does not exist");
  //   }

  //   // Get all posts for the target user
  //   return await _wallPostRepository.GetByTargetUserIdAsync(targetUserId);

  // }

  // public async Task<WallPost> UpdateWallPostAsync(WallPost wallPost)
  // {
  //   //Validate post exists
  //   var _wallPost = await _wallPostRepository.GetByIdAsync(wallPost.Id);

  //   if (wallPost == null)
  //   {
  //     throw new ArgumentException("Post does not exist");
  //   }

  //   //Update post
  //   _wallPost.Content = wallPost.Content;

  //   return await _wallPostRepository.UpdateAsync(_wallPost);

  // }

  // public async Task<bool> DeleteWallPostAsync(int id)
  // {
  //   //Validate post exists
  //   var wallPost = await _wallPostRepository.GetByIdAsync(id);
  //   if (wallPost == null)
  //   {
  //     throw new ArgumentException("Post does not exist");
  //   }
  //   //Delete post
  //   await _wallPostRepository.DeleteAsync(id);
  //   return true;
  // }

}
