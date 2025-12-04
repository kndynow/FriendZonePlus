using System;

namespace FriendZonePlus.Core.Entities;

public class Message
{
  public int Id { get; set; }
  public int SenderId { get; set; }
  public int ReceiverId { get; set; }
  public string Content { get; set; } = string.Empty;
  public DateTime SentAt { get; set; } = DateTime.UtcNow;
  public bool IsRead { get; set; } = false;

}
