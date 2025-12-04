using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Helpers;

public interface IJwtHelper
{
    string GenerateToken(User user);
}
