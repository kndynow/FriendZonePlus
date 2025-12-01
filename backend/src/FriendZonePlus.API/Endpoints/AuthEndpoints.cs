

public static class AuthEndpoints
{
  public static void MapUserEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/Users")
                    .WithTags("Users");

    // group.MapPost("/register", CreateUser);
  }

  // //CREATE
  // private static async Task<Results<Ok<object>, BadRequest<object>>> CreateUser(
  //         AuthorizationService authService,
  //         [FromBody] CreateUserDto dto)
  // {
  //   try
  //   {
  //     var userId = await authService.CreateUserAsync(dto);
  //     return TypedResults.Ok<object>(new { Id = userId, Message = "Created" });
  //   }
  //   catch (ArgumentException ex)
  //   {
  //     return TypedResults.BadRequest<object>(new { Error = ex.Message });
  //   }
  }