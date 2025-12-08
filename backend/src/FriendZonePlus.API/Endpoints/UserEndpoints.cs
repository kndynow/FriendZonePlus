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

    //Get user profile
    group.MapGet("/{id}", async (int id, [FromServices] IUserService userservice) =>
    {
      var userProfile = await userservice.GetUserProfileAsync(id);

      return TypedResults.Ok(userProfile);
    });

    //Update user profile
    group.MapPut("/me", async (UpdateUserDto dto, ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.UpdateUserProfileAsync(currentUserId, dto);

      return TypedResults.Ok(new { message = "User profile updated successfully" });
    });

    //Delete user
    group.MapDelete("/me", async (ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.DeleteUserAsync(currentUserId);

      return TypedResults.NoContent();
    });

    //Follow user
    group.MapPost("/{id}/follow", async (int id, ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.FollowUserAsync(currentUserId, id);

      return TypedResults.Ok(new { message = "User followed successfully" });
    });

    //Unfollow user
    group.MapDelete("/{id}/unfollow", async (int id, ClaimsPrincipal user, [FromServices] IUserService userService) =>
    {
      var currentUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      await userService.UnfollowUserAsync(currentUserId, id);

      return TypedResults.Ok(new { message = "User unfollowed successfully" });
    });
  }

}
