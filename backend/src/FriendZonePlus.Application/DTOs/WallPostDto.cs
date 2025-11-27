namespace FriendZonePlus.Application.DTOs;

public record CreateWallPostDto(int AuthorId, int TargetUserId, string Content);

public record WallPostResponseDto(int Id, int AuthorId, string Content, DateTime CreatedAt);
