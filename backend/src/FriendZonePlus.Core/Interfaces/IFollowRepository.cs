using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Core.Interfaces;

public interface IFollowRepository
{
    Task AddAsync(Follows follows);
}
