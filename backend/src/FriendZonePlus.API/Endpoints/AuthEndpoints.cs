using FluentValidation;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Helpers.ValidationHelpers;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;

public static class AuthEndpoints
{
  public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/Auth")
                    .WithTags("Authorization");

        group.MapPost("/register", RegisterUser);
    }

    private static async Task<IResult> RegisterUser(
        IAuthorizationService authorizationService,
        IValidator<RegisterUserRequestDto> validator,
        RegisterUserRequestDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);

        if (!validationResult.IsValid)
        {
            var errors = ValidationHelper.ToCamelCaseErrors(validationResult.Errors);
            return Results.BadRequest(new { errors });
        }

        var user = new User
        {
            Username = requestDto.Username,
            Email = requestDto.Email,
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            PasswordHash = requestDto.Password,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            var result = await authorizationService.CreateUserAsync(user);

            return Results.Created($"/api/Auth/{result.Id}", result);
        }

        catch (Exception)
        {
            return Results.BadRequest(new { message = "Unable to create account." });
        }
    }
  }