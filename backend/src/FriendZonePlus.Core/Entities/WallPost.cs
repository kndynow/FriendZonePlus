namespace FriendZonePlus.Core.Entities;

public class WallPost
{
  public int Id { get; set; }

  public string Content { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  public int AuthorId { get; set; }
  public User Author { get; set; } = null!;
  public int TargetUserId { get; set; }
  public User TargetUser { get; set; } = null!;
}
