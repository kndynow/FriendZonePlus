using FluentValidation;
using FriendZonePlus.API.DTOs;
using FriendZonePlus.Application.Helpers.ValidationHelpers;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Auth")
                        .WithTags("Authorization");

        group.MapPost("/register", RegisterUser);
        group.MapPost("/login", Login);
    }

    private static async Task<IResult> RegisterUser(
        IAuthenticationService authenticationService,
        [FromServices] IValidator<RegisterUserRequestDto> validator,
        RegisterUserRequestDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);

        if (!validationResult.IsValid)
        {
            var errors = ValidationHelper.ToCamelCaseErrors(validationResult.Errors);
            return Results.BadRequest(new { errors });
        }

        try
        {
            var user = new User
            {
                Username = requestDto.Username,
                Email = requestDto.Email,
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                PasswordHash = requestDto.Password,
                CreatedAt = DateTime.UtcNow
            };

            var result = await authenticationService.CreateUserAsync(user);

            return Results.Created($"/api/Auth/{result.Id}", result);
        }

        catch (Exception)
        {
            return Results.BadRequest(new { message = "Unable to create account." });
        }
    }

    private static async Task<IResult> Login(
        IAuthenticationService authenticationService,
        LoginRequestDto requestDto)
    {

        if (string.IsNullOrEmpty(requestDto.UsernameOrEmail) ||
        string.IsNullOrEmpty(requestDto.Password))
        {
            return Results.BadRequest(new { message = "Username or email and password are required." });
        }

        var token = await authenticationService.LoginAsync(requestDto.UsernameOrEmail, requestDto.Password);

        if (token == null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new { token });
    }
}