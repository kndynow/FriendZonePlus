namespace FriendZonePlus.Core.Entities;

public class Follows
{
  public int Id { get; set; }
  public int FolloweeId { get; set; }
  public User Followee { get; set; } = null!;
  public int FollowerId { get; set; }
  public User Follower { get; set; } = null!;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
