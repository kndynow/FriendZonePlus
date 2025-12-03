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
    group.MapGet("/target/{targetUserId}", GetWallPostsForTargetUser);
    group.MapGet("/author/{authorId}", GetWallPostsForAuthor);
    group.MapGet("/feed/{userId}", GetFeedForUser);
    group.MapPatch("/update", UpdateWallPost);
    // group.MapDelete("/delete/{id}", DeleteWallPost);
  }

  //TODO: Map to entity with Mapster
  //TODO: Add validation to dto
  //TODO: Global exception handler (maybe)
  // CREATE
  private static async Task<Results<Ok<WallPostResponseDto>, BadRequest<ErrorResponseDto>>> CreateWallPost(
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
      return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
    }
  }

  //GET ALL WALL POSTS FOR A TARGET USER
  private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<ErrorResponseDto>>> GetWallPostsForTargetUser(
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
      return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
    }
  }


  // GET ALL WALL POSTS FOR AN AUTHOR
  private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<ErrorResponseDto>>> GetWallPostsForAuthor(
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
      return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
    }
  }

  //GET ALL WALL POSTS FOR A USER'S FEED
  private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<ErrorResponseDto>>> GetFeedForUser(
            WallPostService wallPostService,
            int userId)
  {
    try
    {
      var result = await wallPostService.GetFeedForUserAsync(userId);
      return TypedResults.Ok(result);
    }
    catch (ArgumentException ex)
    {
      return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
    }
  }

  // PATCH
  private static async Task<Results<Ok<WallPostResponseDto>, BadRequest<ErrorResponseDto>>> UpdateWallPost(
            WallPostService wallPostService,
            [FromBody] UpdateWallPostDto dto)
  {
    try
    {
      var result = await wallPostService.UpdateWallPostAsync(dto);
      return TypedResults.Ok(result);
    }
    catch (ArgumentException ex)
    {
      return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
    }
  }

  //DELETE
  private static async Task<Results<NoContent, NotFound>> DeleteWallPost(
            WallPostService wallPostService,
            int id)
  {
    var success = await wallPostService.DeleteWallPostAsync(id);
    if (!success)
    {
      return TypedResults.NotFound();
    }
    return TypedResults.NoContent();
  }

}