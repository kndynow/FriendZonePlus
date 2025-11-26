using System;

namespace FriendZonePlus.Core.Entities;

public class User
{
  public int Id { get; set; }
  public string Username { get; set; }
  public string Email { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
