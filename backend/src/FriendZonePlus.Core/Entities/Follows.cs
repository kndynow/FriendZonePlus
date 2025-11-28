using System;
using System.Net;
using FriendZonePlus.Core.Enums;

namespace FriendZonePlus.Core.Entities;

public class Follows
{
  public int Id { get; set; }
  public int FolloweeId { get; set; }
  public User Followee { get; set; } = null!;
  public int FollowerId { get; set; }
  public User Follower { get; set; } = null!;
  public Status Status { get; set; } = Status.Pending;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
