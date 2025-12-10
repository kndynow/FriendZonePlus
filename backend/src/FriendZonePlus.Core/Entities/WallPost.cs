namespace FriendZonePlus.Core.Entities;

public class WallPost
{
  public int Id { get; set; }
  public string Content { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  //Relationships
  //User who authored the post
  public int AuthorId { get; set; }
  public User Author { get; set; } = null!;
  //Target user for post
  public int TargetUserId { get; set; }
  public User TargetUser { get; set; } = null!;
}
