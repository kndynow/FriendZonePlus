using FriendZonePlus.API.Filters;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services.Authentication;
using FriendZonePlus.Infrastructure.Authentication;
using FriendZonePlus.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Auth")
                        .WithTags("Authorization");


        group.MapPost("/register", async (RegisterRequest request, [FromServices] IAuthenticationService authService) =>
        {
            var response = await authService.RegisterAsync(request);
            return TypedResults.Created($"/api/Auth/{response.UserId}", response);
        })
        .WithDescription("Registers a new user account with the provided information")
        .WithSummary("Register new user")
        .AddEndpointFilter<ValidationFilter<RegisterRequest>>();

        group.MapPost("/login", async (LoginRequest request, [FromServices] IAuthenticationService
        authenticationService,
        HttpContext httpContext,
        IOptions<JwtSettings> jwtOptions) =>
        {
            var response = await authenticationService.LoginAsync(request);
            var jwtSettings = jwtOptions.Value;

            httpContext.Response.Cookies.Append("auth", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // MUST BE TRUE IN PRODUCTION
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpirationMinutes)
            });

            return TypedResults.Ok(new
            {
                response.UserId,
                response.FirstName,
                response.LastName,
                response.Username,
                response.Email
            });
        })
        .WithDescription("Authenticates a user with email and password. Returns user information and sets an authentication cookie")
        .WithSummary("Login")
        .AddEndpointFilter<ValidationFilter<LoginRequest>>();

        group.MapPost("/logout", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Delete("auth");
            return TypedResults.Ok();
        })
        .WithDescription("Logs out the current user by removing the authentication cookie")
        .WithSummary("Logout");

        // Validateas that the user has an active valid token
        group.MapGet("/me", (HttpContext httpContext) =>
        {
            var token = httpContext.Request.Cookies["auth"];

            if (string.IsNullOrEmpty(token))
                return TypedResults.Unauthorized();

            var validator = httpContext.RequestServices.GetRequiredService<ITokenValidator>();

            var principal = validator.Validate(token);

            if (principal == null)
                return TypedResults.Unauthorized();

            // Try multiple claim types to find the values
            var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? principal.FindFirst("sub")?.Value;

            var username = principal.FindFirst("username")?.Value;

            var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value
                     ?? principal.FindFirst(ClaimTypes.Email)?.Value
                     ?? principal.FindFirst("email")?.Value;

            return (IResult)TypedResults.Ok(new { UserId = userId, Username = username, Email = email });
        })
        .WithDescription("Validates the authentication token and returns the current authenticated user's information")
        .WithSummary("Get current user");
    }
}