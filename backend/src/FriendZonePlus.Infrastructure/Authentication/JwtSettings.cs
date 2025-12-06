
namespace FriendZonePlus.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string SecretKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int TokenExpirationMinutes { get; init; }

}
