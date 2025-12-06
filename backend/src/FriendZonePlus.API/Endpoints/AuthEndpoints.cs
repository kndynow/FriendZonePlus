using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services.Authentication;
using FriendZonePlus.API.Filters;

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
        authenticationService) =>
        {
            var response = await authenticationService.LoginAsync(request);
            return TypedResults.Ok(response);
        }).AddEndpointFilter<ValidationFilter<LoginRequest>>();
    }
}