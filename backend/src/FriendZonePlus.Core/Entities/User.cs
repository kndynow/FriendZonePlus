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

}
