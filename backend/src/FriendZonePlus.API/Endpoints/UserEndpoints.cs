using System.Security.Claims;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FriendZonePlus.API.Endpoints;

public static class UserEndpoints
{
  public static void MapUserEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/users")
                    .WithTags("Users");

    //Get users
    group.MapGet("/", async ([FromServices] IUserService userService) =>
    {
      var users = await userService.GetAllUsersAsync();
      return TypedResults.Ok(users);
    })
    .WithDescription("Gets a list of all users")
    .WithSummary("Get users");

    group.MapGet("/with-following-status", async (ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var users = await userService.GetAllUsersWithFollowingStatusAsync(currentUserId);
      return TypedResults.Ok(users);
    })
    .WithDescription("Gets all users with following status for the authenticated user")
    .WithSummary("Get users with following status");

    //Get user profile
    group.MapGet("/{id}", async (int id, [FromServices] IUserService userservice) =>
    {
      var userProfile = await userservice.GetUserProfileAsync(id);

      return TypedResults.Ok(userProfile);
    })
    .WithDescription("Gets the user profile information for a specific user by their user ID")
    .WithSummary("Get user profile");

    //Update user profile
    group.MapPut("/me", async (UpdateUserDto dto, ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.UpdateUserProfileAsync(currentUserId, dto);

      return TypedResults.Ok(new { message = "User profile updated successfully" });
    })
    .WithDescription("Updates the profile information for the authenticated user")
    .WithSummary("Update user profile");

    //Delete user
    group.MapDelete("/me", async (ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.DeleteUserAsync(currentUserId);

      return TypedResults.NoContent();
    })
    .WithDescription("Deletes the authenticated user's account")
    .WithSummary("Delete user account");


    //Follow user
    group.MapPost("/{id}/follow", async (int id, ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.FollowUserAsync(currentUserId, id);

      return TypedResults.Ok(new { message = "User followed successfully" });
    })
    .WithDescription("Follows a user by their user ID. The authenticated user will start following the specified user")
    .WithSummary("Follow user");

    //Unfollow user
    group.MapDelete("/{id}/unfollow", async (int id, ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.UnfollowUserAsync(currentUserId, id);

      return TypedResults.Ok(new { message = "User unfollowed successfully" });
    })
    .WithDescription("Unfollows a user by their user ID. The authenticated user will stop following the specified user")
    .WithSummary("Unfollow user");

    //Get followers
    group.MapGet("/me/followers", async (ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      var followers = await userService.GetFollowersAsync(currentUserId);

      return TypedResults.Ok(followers);
    })
    .WithDescription("Gets a list of all users that are following the authenticated user")
    .WithSummary("Get followers");

    //Get following
    group.MapGet("/me/following", async (ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      var following = await userService.GetFollowingAsync(currentUserId);

      return TypedResults.Ok(following);
    })
    .WithDescription("Gets a list of all users that the authenticated user is following")
    .WithSummary("Get following");
  }

}
