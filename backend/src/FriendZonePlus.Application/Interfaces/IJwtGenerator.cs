using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(User user);
}
