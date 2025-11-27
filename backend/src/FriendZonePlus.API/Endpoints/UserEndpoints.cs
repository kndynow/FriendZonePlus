using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FriendZonePlus.API.Endpoints;

public static class UserEndpoints
{
  public static void MapUserEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/Users")
                    .WithTags("Users");

    group.MapPost("/register", CreateUser);
    group.MapGet("/{id}", GetUserById);
    group.MapDelete("/{id}", DeleteUser);
  }

  private static async Task<Results<Ok<object>, BadRequest<object>>> CreateUser(
          UserService userService,
          [FromBody] CreateUserDto dto)
  {
    try
    {
      var userId = await userService.CreateUserAsync(dto);
      return TypedResults.Ok<object>(new { Id = userId, Message = "Created" });
    }
    catch (ArgumentException ex)
    {
      return TypedResults.BadRequest<object>(new { Error = ex.Message });
    }
  }

  //GET
  private static async Task<Results<Ok<object>, NotFound>> GetUserById(
        int id,
        UserService userService)
  {
    var user = await userService.GetUserByIdAsync(id);

    if (user is null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok<object>(new { user.Id, user.Username, user.Email });
  }

  //DELETE
  private static async Task<Results<NoContent, NotFound>> DeleteUser(int id, UserService userService)
  {
    var success = await userService.DeleteUserAsync(id);

    if (!success)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.NoContent();
  }
}
