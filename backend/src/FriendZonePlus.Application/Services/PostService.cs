using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class PostService
{
  private readonly IPostRepository _postRepository;
  private readonly IUserRepository _userRepository;

  public PostService(IPostRepository postRepository, IUserRepository userRepository)
  {
    _postRepository = postRepository;
    _userRepository = userRepository;
  }


  public async Task<PostDtos.Response> CreatePostAsync(PostDtos.Create dto)
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

    var post = new Post
    {
      AuthorId = dto.AuthorId,
      TargetUserId = dto.TargetUserId,
      Content = dto.Content,
      CreatedAt = DateTime.UtcNow
    };

    var createdPost = await _postRepository.AddAsync(post);

    return new PostDtos.Response(
        createdPost.Id,
        createdPost.AuthorId,
        createdPost.Content,
        createdPost.CreatedAt
      );
  }
}
