using FluentValidation;
using FriendZonePlus.API.DTOs;
using FriendZonePlus.Application.Helpers.ValidationHelpers;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Services.Authentication;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Auth")
                        .WithTags("Authorization");

        group.MapPost("/register", RegisterUser);
    }

    private static async Task<IResult> RegisterUser(
        IAuthenticationService authenticationService,
        IValidator<RegisterUserRequestDto> validator,
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
}