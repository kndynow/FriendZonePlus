using System;
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


  public async Task<WallPost> CreateWallPostAsync(WallPost wallPost)
  {

    //Validate author exists
    var author = await _userRepository.GetByIdAsync(wallPost.AuthorId);
    if (author == null)
      throw new ArgumentException("Author does not exist");

    //Validate target user exists
    var target = await _userRepository.GetByIdAsync(wallPost.TargetUserId);
    if (target == null)
      throw new ArgumentException("Target user does not exist");

    //Add post to database
    return await _wallPostRepository.AddAsync(wallPost);
  }

  public async Task<IEnumerable<WallPost>> GetFeedForUserAsync(int userId)
  {
    //Validate user exists
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null)
    {
      throw new ArgumentException("User does not exist");
    }

    // Get IDs for all users the current user follows
    var followedUserIds = await _followRepository.GetFollowedUserIdsAsync(userId);


    return await _wallPostRepository.GetFeedForUserAsync(followedUserIds);
  }

  public async Task<IEnumerable<WallPost>> GetWallPostsForAuthorAsync(int authorId)
  {
    //Validate author exists
    var author = await _userRepository.GetByIdAsync(authorId);
    if (author == null)
    {
      throw new ArgumentException("Author does not exist");
    }

    // Get all posts for the author
    return await _wallPostRepository.GetByAuthorIdAsync(authorId);
  }
  public async Task<IEnumerable<WallPost>> GetWallPostsForTargetUserAsync(int targetUserId)
  {
    //Validate target user exists
    var target = await _userRepository.GetByIdAsync(targetUserId);
    if (target == null)
    {
      throw new ArgumentException("Target user does not exist");
    }

    // Get all posts for the target user
    return await _wallPostRepository.GetByTargetUserIdAsync(targetUserId);

  }

  public async Task<WallPost> UpdateWallPostAsync(WallPost wallPost)
  {
    //Validate post exists
    var _wallPost = await _wallPostRepository.GetByIdAsync(wallPost.Id);

    if (wallPost == null)
    {
      throw new ArgumentException("Post does not exist");
    }

    //Update post
    _wallPost.Content = wallPost.Content;

    return await _wallPostRepository.UpdateAsync(_wallPost);

  }

  public async Task<bool> DeleteWallPostAsync(int id)
  {
    //Validate post exists
    var wallPost = await _wallPostRepository.GetByIdAsync(id);
    if (wallPost == null)
    {
      throw new ArgumentException("Post does not exist");
    }
    //Delete post
    await _wallPostRepository.DeleteAsync(id);
    return true;
  }

}
