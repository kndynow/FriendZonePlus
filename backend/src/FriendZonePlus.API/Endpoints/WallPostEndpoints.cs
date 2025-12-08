using System;
using System.Security.Claims;
using FriendZonePlus.API.Filters;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FriendZonePlus.API.Endpoints;

public static class WallPostEndpoints
{
  public static void MapWallPostEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/wallposts").WithTags("WallPost");

    // Create wall post
    group.MapPost("/", async (
      CreateWallPostDto dto,
      ClaimsPrincipal user,
      IWallPostService wallPostService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var createdPost = await wallPostService.CreateAsync(currentUserId, dto);
      return TypedResults.Created($"/api/wallposts/{createdPost.Id}", createdPost);
    }).AddEndpointFilter<ValidationFilter<CreateWallPostDto>>();

    // Update wall post
    group.MapPut("/{id}", async (
      int id,
      UpdateWallPostDto dto,
      ClaimsPrincipal user,
      IWallPostService wallPostService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      await wallPostService.UpdateWallPostAsync(currentUserId, id, dto);
      return TypedResults.Ok(new { message = "Wall post updated successfully" });
    }).AddEndpointFilter<ValidationFilter<UpdateWallPostDto>>();

    // Delete wall post
    group.MapDelete("/{id}", async (
      int id,
      ClaimsPrincipal user,
      IWallPostService wallPostService) =>
    {

      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      await wallPostService.DeleteWallPostAsync(currentUserId, id);
      return TypedResults.NoContent();
    });

    // Get feed
    group.MapGet("/feed", async (
      ClaimsPrincipal user,
      IWallPostService wallPostService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var feed = await wallPostService.GetFeedAsync(currentUserId);
      return TypedResults.Ok(feed);
    });

    // Get wall posts for target user
    app.MapGet("/users/{id}/wall", async (int id, IWallPostService wallpostService) =>
    {
      var wallPosts = await wallpostService.GetWallPostsAsync(id);
      return TypedResults.Ok(wallPosts);
    });

  }
}










// // CREATE
// private static async Task<Results<Ok<WallPostResponseDto>, BadRequest<ErrorResponseDto>>> CreateWallPost(
//         WallPostService wallPostService,
//         CreateWallPostDto dto)
// {
//   var wallPost = dto.Adapt<WallPost>();
//   try
//   {
//     var result = await wallPostService.CreateWallPostAsync(wallPost);

//     var responseDto = result.Adapt<WallPostResponseDto>();

//     return TypedResults.Ok(responseDto);
//   }
//   catch (ArgumentException ex)
//   {
//     return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
//   }
// }

// //GET ALL WALL POSTS FOR A TARGET USER
// private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<ErrorResponseDto>>> GetWallPostsForTargetUser(
//           WallPostService wallPostService,
//           int targetUserId)
// {
//   try
//   {
//     var result = await wallPostService.GetWallPostsForTargetUserAsync(targetUserId);
//     var responseDto = result.Adapt<IEnumerable<WallPostResponseDto>>();
//     return TypedResults.Ok(responseDto);
//   }
//   catch (ArgumentException ex)
//   {
//     return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
//   }
// }


// // GET ALL WALL POSTS FOR AN AUTHOR
// private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<ErrorResponseDto>>> GetWallPostsForAuthor(
//           WallPostService wallPostService,
//           int authorId)
// {
//   try
//   {
//     var result = await wallPostService.GetWallPostsForAuthorAsync(authorId);
//     var responseDto = result.Adapt<IEnumerable<WallPostResponseDto>>();
//     return TypedResults.Ok(responseDto);
//   }
//   catch (ArgumentException ex)
//   {
//     return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
//   }
// }

// //GET ALL WALL POSTS FOR A USER'S FEED
// private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, BadRequest<ErrorResponseDto>>> GetFeedForUser(
//           WallPostService wallPostService,
//           int userId)
// {
//   try
//   {
//     var result = await wallPostService.GetFeedForUserAsync(userId);
//     var responseDto = result.Adapt<IEnumerable<WallPostResponseDto>>();
//     return TypedResults.Ok(responseDto);
//   }
//   catch (ArgumentException ex)
//   {
//     return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
//   }
// }

// // PATCH
// private static async Task<Results<Ok<WallPostResponseDto>, BadRequest<ErrorResponseDto>>> UpdateWallPost(
//           WallPostService wallPostService,
//           UpdateWallPostDto dto)
// {
//   try
//   {
//     var wallPost = dto.Adapt<WallPost>();
//     var result = await wallPostService.UpdateWallPostAsync(wallPost);
//     var responseDto = result.Adapt<WallPostResponseDto>();
//     return TypedResults.Ok(responseDto);
//   }
//   catch (ArgumentException ex)
//   {
//     return TypedResults.BadRequest(new ErrorResponseDto(ex.Message));
//   }
// }

// //DELETE
// private static async Task<Results<NoContent, NotFound>> DeleteWallPost(
//           WallPostService wallPostService,
//           int id)
// {
//   var success = await wallPostService.DeleteWallPostAsync(id);
//   if (!success)
//   {
//     return TypedResults.NotFound();
//   }
//   return TypedResults.NoContent();
// }

