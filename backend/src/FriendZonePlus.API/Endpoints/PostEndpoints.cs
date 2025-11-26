using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FriendZonePlus.API.Endpoints;

public static class PostEndpoints
{
  public static void MapPostEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/posts").WithTags("Post");

    group.MapPost("/", CreatePost);
  }

  private static async Task<Results<Ok<PostDtos.Response>, BadRequest<object>>> CreatePost(
          PostService postService,
          PostDtos.Create dto)
  {
    try
    {
      var result = await postService.CreatePostAsync(dto);

      return TypedResults.Ok(result);
    }
    catch (ArgumentException ex)
    {
      return TypedResults.BadRequest<object>(new { Error = ex.Message });
    }
  }
}
