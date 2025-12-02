using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

public static class AuthEndpoints
{
  public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/Authorization")
                    .WithTags("Authorization");

        group.MapPost("/register", RegisterUser);
    }

    private static async Task<IResult> RegisterUser(
        IAuthorizationService authorizationService,
        RegisterUserRequestDto requestDto)
    {
        try
        {
            var result = await authorizationService.CreateUserAsync(requestDto);

            return Results.Created($"/api/Authorization/{result.Id}", result);
        }
        catch (ValidationException ex)
        {            
            return Results.BadRequest(new
            {
                errors = ex.Errors.Select(e => e.ErrorMessage)
            });
        }
     }
  }