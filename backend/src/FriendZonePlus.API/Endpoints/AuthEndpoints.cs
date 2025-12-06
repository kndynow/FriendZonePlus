using FriendZonePlus.API.Filters;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services.Authentication;
using FriendZonePlus.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Auth")
                        .WithTags("Authorization");


        group.MapPost("/register", async (RegisterRequest request, IAuthenticationService authService) =>
        {
            var response = await authService.RegisterAsync(request);
            return TypedResults.Created($"/api/Auth/{response.UserId}", response);
        }).AddEndpointFilter<ValidationFilter<RegisterRequest>>();

        group.MapPost("/login", async (LoginRequest request, IAuthenticationService
        authenticationService,
        HttpContext httpContext,
        IOptions<JwtSettings> jwtOptions) =>
        {
            var response = await authenticationService.LoginAsync(request);
            var jwtSettings = jwtOptions.Value;

            httpContext.Response.Cookies.Append("auth", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Must be true in prod
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpirationMinutes)
            });

            return TypedResults.Ok(new
            {
                response.UserId,
                response.Username
            });
        }).AddEndpointFilter<ValidationFilter<LoginRequest>>();

        group.MapPost("/logout", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Delete("auth");
            return TypedResults.Ok();
        });
    }


}