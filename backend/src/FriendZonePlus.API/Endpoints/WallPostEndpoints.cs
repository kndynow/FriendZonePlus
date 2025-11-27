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

  // Create new wall post
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
}

// Get all wall posts for a target user

// private static async Task<Results<Ok<IEnumerable<WallPostResponseDto>>, NotFound>> GetWallPostsForTargetUser(
//   int targetUserId,
//   WallPostService wallPostService)
//   {
//     var wallPosts = await wallPostService.GetWallPostsForTargetUserAsync(targetUserId);
//     return TypedResults.Ok(wallPosts);
//   }
// catch (ArgumentException ex)
// {
//   return TypedResults.BadRequest<object>(new { Error = ex.Message });
// }
// };
