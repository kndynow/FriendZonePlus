using System;

namespace FriendZonePlus.Core.Entities;

public class User
{
  public int Id { get; set; }
  public string Username { get; set; }
  public string Email { get; set; }
  public string PasswordHash { get; set; } = string.Empty;
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string ProfilePictureUrl { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  //Relationships
  //Posts authored by current user
  public ICollection<WallPost> AuthoredPosts { get; set; } = [];
  // Posts on current user's wall
  public ICollection<WallPost> WallPosts { get; set; } = [];
  //Users following current user
  public ICollection<Follow> Followers { get; set; } = [];
  //Users current user is following
  public ICollection<Follow> Following { get; set; } = [];

}
