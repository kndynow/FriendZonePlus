using System;

namespace FriendZonePlus.Core.Entities;

public class Follows
{
  public int follower_id { get; set; }
  public int followee_id { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
