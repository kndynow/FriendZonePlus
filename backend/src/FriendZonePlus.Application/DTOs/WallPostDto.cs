using System.ComponentModel.DataAnnotations;

namespace FriendZonePlus.Application.DTOs;

public record CreateWallPostDto(
    int TargetUserId,
    string Content
    );

public record WallPostResponseDto(
    int Id,
    string Content,
    DateTime CreatedAt,
    int AuthorId,
    string AuthorName,
    int TargetUserId,
    string TargetUserName
    );

public record UpdateWallPostDto(string Content);

