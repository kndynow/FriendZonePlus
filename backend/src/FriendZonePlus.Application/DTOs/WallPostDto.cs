using System.ComponentModel.DataAnnotations;

namespace FriendZonePlus.Application.DTOs;

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
    string Content,
    DateTime CreatedAt
    );

public record UpdateWallPostDto(
    [Required]
    int Id,
    string Content,
    int AuthorId,
    DateTime CreatedAt
    );
