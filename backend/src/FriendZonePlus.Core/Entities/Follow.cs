namespace FriendZonePlus.Core.Entities;

public class Follow
{
  public int Id { get; set; }

  // The user who follows
  public int FollowerId { get; set; }
  public User Follower { get; set; } = null!;

  // The user who is being followed
  public int FollowedUserId { get; set; }
  public User FollowedUser { get; set; } = null!;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
