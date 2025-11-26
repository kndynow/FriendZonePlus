
namespace FriendZonePlus.Application.DTOs;

public static class PostDtos
{
  public record Create(int AuthorId, int TargetUserId, string Content);

  public record Response(int Id, int AuthorId, string Content, DateTime CreatedAt);

}
