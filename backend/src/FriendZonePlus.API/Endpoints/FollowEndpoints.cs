using System;
using FriendZonePlus.API.DTOs;
using FriendZonePlus.Application.Services;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FriendZonePlus.API.Endpoints;

public static class FollowEndpoints
{
    public static void MapFollowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Follows").WithTags("Follows");

        group.MapPost("/follow", FollowUser);
        group.MapPost("/unfollow", UnfollowUser);
        group.MapGet("/followers/{userId}", GetFollowers);
        group.MapGet("/followed/{userId}", GetFollowing);
    }

    private static async Task<Results<Created<FollowResponseDto>, BadRequest<ErrorResponseDto>>> FollowUser(
        FollowService followService,
        FollowUserRequestDto dto)
    {
        try
        {
            var createdFollow = await followService.FollowAsync(dto.FollowerId, dto.FollowedUserId);
            var responseDto = createdFollow.Adapt<FollowResponseDto>();
            return TypedResults.Created($"/api/Follows/{dto.FollowerId}/{dto.FollowedUserId}", responseDto);
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
        }
    }

    private static async Task<Results<NoContent, BadRequest<ErrorResponseDto>>> UnfollowUser(
        FollowService followService,
        int followerId,
        int followedUserId)
    {
        try
        {
            await followService.UnfollowAsync(followerId, followedUserId);
            return TypedResults.NoContent();
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
        }
    }

    private static async Task<Results<Ok<IEnumerable<UserListResponseDto>>, BadRequest<ErrorResponseDto>>> GetFollowers(
    FollowService followService,
    int userId)
    {
        try
        {
            var followers = await followService.GetFollowersAsync(userId);
            var responseDto = followers.Adapt<IEnumerable<UserListResponseDto>>();
            return TypedResults.Ok(responseDto);
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
        }
    }

    private static async Task<Results<Ok<IEnumerable<UserListResponseDto>>, BadRequest<ErrorResponseDto>>> GetFollowing(
    FollowService followService,
    int userId)
    {
        try
        {
            var following = await followService.GetFollowedUsersAsync(userId);
            var responseDto = following.Adapt<IEnumerable<UserListResponseDto>>();
            return TypedResults.Ok(responseDto);
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
        }
    }

}
