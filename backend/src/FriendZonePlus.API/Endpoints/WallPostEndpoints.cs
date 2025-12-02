using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FriendZonePlus.API.Endpoints;

public static class WallPostEndpoints
{
  public static void MapWallPostEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/WallPosts").WithTags("WallPost");

    group.MapPost("/create", CreateWallPost);
  }

  // CREATE
  private static async Task<Results<Ok<WallPostResponseDto>, BadRequest<object>>> CreateWallPost(
          WallPostService wallPostService,
          [FromBody] CreateWallPostDto dto)
  {
    try
    {
      var result = await wallPostService.CreateWallPostAsync(dto);

      return TypedResults.Ok(result);
    }
    catch (ArgumentException ex)
    {
      return TypedResults.BadRequest<object>(new { Error = ex.Message });
    }
  }

  //TODO: GET ALL WALL POSTS FOR A TARGET USER
  private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<object>>> GetWallPostsForTargetUser(
            WallPostService wallPostService,
            int targetUserId)
  {
    try
    {
      var result = await wallPostService.GetWallPostsForTargetUserAsync(targetUserId);
      return TypedResults.Ok(result);
    }
    catch (ArgumentException ex)
    {
      return TypedResults.BadRequest<object>(new { Error = ex.Message });
    }
  }


  //TODO: GET ALL WALL POSTS FOR AN AUTHOR
  private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<object>>> GetWallPostsForAuthor(
            WallPostService wallPostService,
            int authorId)
  {
    try
    {
      var result = await wallPostService.GetWallPostsForAuthorAsync(authorId);
      return TypedResults.Ok(result);
    }
    catch (ArgumentException ex)
    {
      return TypedResults.BadRequest<object>(new { Error = ex.Message });
    }
  }


  //TODO: GET ALL WALL POSTS FOR A USER'S FEED

  //TODO: PATCH

  //TODO: DELETE

}