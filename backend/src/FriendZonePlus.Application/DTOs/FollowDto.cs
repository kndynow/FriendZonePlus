using System;
using System.ComponentModel.DataAnnotations;

namespace FriendZonePlus.Application.DTOs;

public record FollowUserRequestDto(
    [Required, Range(1, int.MaxValue, ErrorMessage = "Follower Id must be greater than 0") ]
    int FollowerId,
    [Required, Range(1, int.MaxValue, ErrorMessage = "Followed user Id must be greater than 0") ]
    int FollowedUserId
    );

public record UnfollowUserRequestDto(
    [Required, Range(1, int.MaxValue, ErrorMessage = "Follower Id must be greater than 0") ]
    int FollowerId,
    [Required, Range(1, int.MaxValue, ErrorMessage = "Followed user Id must be greater than 0") ]
    int FollowedUserId
    );

public record FollowResponseDto(
    int Id,
    int FollowerId,
    int FollowedUserId,
    DateTime CreatedAt
);

public record UserListResponseDto(
    int Id,
    string Username,
    string Email
);

