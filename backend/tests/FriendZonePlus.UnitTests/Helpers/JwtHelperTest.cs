using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FriendZonePlus.Application.Helpers.JwtHelper;
using FriendZonePlus.Core.Entities;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FriendZonePlus.UnitTests.Helpers;

public class JwtHelperTests
{

    private readonly IConfiguration _configuration;
    private readonly JwtHelper _jwtHelper;

    public JwtHelperTests()
    {
        // Setup in-memory configuration
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "Jwt:SecretKey", "test-secret-key-that-is-at-least-32-characters-long-for-hs256" },
            { "Jwt:Issuer", "test-issuer" },
            { "Jwt:Audience", "test-audience" },
            { "Jwt:TokenExpirationMinutes", "60" }
        });
        _configuration = configurationBuilder.Build();
        _jwtHelper = new JwtHelper(_configuration);
    }

    private User CreateTestUser()
    {
        return new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hashed",
            CreatedAt = DateTime.UtcNow
        };
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var token = _jwtHelper.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);

        // Verify valid JWT format separated with dots
        var parts = token.Split('.');
        Assert.Equal(3, parts.Length);
    }

    [Fact]
    public void GenerateToken_ShouldContainCorrectClaims()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var token = _jwtHelper.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        Assert.Equal(user.Id.ToString(), jsonToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value
        );
        Assert.Equal(user.Username, jsonToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Equal(user.Email, jsonToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
    }

    [Fact]
    public void GenerateToken_ShouldGenerateDifferentTokensForDifferentUsers()
    {
        // Arrange
        var user1 = CreateTestUser();
        var user2 = CreateTestUser();
        user2.Id = 2;
        user2.Username = "testuser2";

        // Act
        var token1 = _jwtHelper.GenerateToken(user1);
        var token2 = _jwtHelper.GenerateToken(user2);

        // Assert
        Assert.NotEqual(token1, token2);

    }

    [Fact]
    public void GenerateToken_ShouldGenerateDifferentTokens_ForSameUser_WhenCalledMultipleTimes()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var token1 = _jwtHelper.GenerateToken(user);
        var token2 = _jwtHelper.GenerateToken(user);

        // Assert
        // Tokens should be different because each token has a unique JTI (JWT ID)
        Assert.NotEqual(token1, token2);
    }

}
