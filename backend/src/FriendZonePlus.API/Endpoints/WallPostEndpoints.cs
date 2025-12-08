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
      [FromServices] IWallPostService wallPostService) =>
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
      [FromServices] IWallPostService wallPostService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      await wallPostService.UpdateWallPostAsync(currentUserId, id, dto);
      return TypedResults.Ok(new { message = "Wall post updated successfully" });
    }).AddEndpointFilter<ValidationFilter<UpdateWallPostDto>>();

    // Delete wall post
    group.MapDelete("/{id}", async (
      int id,
      ClaimsPrincipal user,
      [FromServices] IWallPostService wallPostService) =>
    {

      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      await wallPostService.DeleteWallPostAsync(currentUserId, id);
      return TypedResults.NoContent();
    });

    // Get feed
    group.MapGet("/feed", async (
      ClaimsPrincipal user,
      [FromServices] IWallPostService wallPostService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var feed = await wallPostService.GetFeedAsync(currentUserId);
      return TypedResults.Ok(feed);
    });

    // Get wall posts for target user
    app.MapGet("/users/{id}/wall", async (int id, [FromServices] IWallPostService wallpostService) =>
    {
      var wallPosts = await wallpostService.GetWallPostsAsync(id);
      return TypedResults.Ok(wallPosts);
    });

  }
}

