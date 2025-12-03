using System.ComponentModel.DataAnnotations;

namespace FriendZonePlus.API.DTOs;

public record CreateWallPostDto(
    [Required]
    int AuthorId,
    int TargetUserId,
    [Required, MaxLength(300), MinLength(1)]
    string Content
    );

public record WallPostResponseDto(
    int Id,
    int AuthorId,
    int TargetUserId,
    string Content,
    DateTime CreatedAt
    );

public record UpdateWallPostDto(
    [Required]
    int Id,
    string Content,
    int AuthorId
    );
