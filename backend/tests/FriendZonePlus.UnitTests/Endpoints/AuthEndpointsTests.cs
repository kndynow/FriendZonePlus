using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using FriendZonePlus.API.DTOs;
using Xunit;

namespace FriendZonePlus.UnitTests.Endpoints;

public class AuthEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AuthEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturnCreated_WhenValidData()
    {
        // Arrange
        var registerRequest = new RegisterUserRequestDto(
            Username: $"testuser_{Guid.NewGuid()}",
            Email: $"test_{Guid.NewGuid()}@example.com",
            FirstName: "Test",
            LastName: "User",
            Password: "SecurePassword123!"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/register", registerRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<RegisterUserResponseDto>();
        Assert.NotNull(result);
        Assert.Equal(registerRequest.Username, result.Username);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenValidCredentials()
    {
        // Arrange - First register a user
        var registerRequest = new RegisterUserRequestDto(
            Username: $"testuser_{Guid.NewGuid()}",
            Email: $"test_{Guid.NewGuid()}@example.com",
            FirstName: "Test",
            LastName: "User",
            Password: "SecurePassword123!"
        );

        await _client.PostAsJsonAsync("/api/Auth/register", registerRequest);

        var loginRequest = new LoginRequestDto(
            UsernameOrEmail: registerRequest.Username,
            Password: registerRequest.Password
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials()
    {
        // Arrange
        var loginRequest = new LoginRequestDto(
            UsernameOrEmail: "nonexistent",
            Password: "wrongpassword"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}