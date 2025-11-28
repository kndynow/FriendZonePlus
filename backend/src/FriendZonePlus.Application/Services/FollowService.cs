using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using System.Threading.Tasks;

namespace FriendZonePlus.Application.Services;

public class FollowService
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserRepository _userRepository;

    public FollowService(IFollowRepository followRepository, IUserRepository userRepository)
    {
        _followRepository = followRepository;
        _userRepository = userRepository;
    }

    //TODO: Follow user
    public async Task FollowAsync(int followerId, int followeeId)
    {
        
       await ValitadeFollowRequest(followerId, followeeId);

        var follow = new Follows
        {
            FollowerId = followerId,
            FolloweeId = followeeId
        };

        await _followRepository.AddAsync(follow);
    }

    //TODO: Get followers
    //TODO: Unfollow user

    private async Task ValitadeFollowRequest(int followerId, int followeeId)
    {
        ValidateSelfFollow(followerId, followeeId);
        await ValidateUniqueFollow(followerId, followeeId);
    }

    private void ValidateSelfFollow(int followerId, int followeeId)
    {
        if (followerId == followeeId)
        {
            throw new InvalidOperationException("User cannot follow themselves.");
        }
    }
    private async Task ValidateUniqueFollow(int followerId, int followeeId)
    {
        if (await _followRepository.ExistsAsync(followerId, followeeId))
        {
            throw new InvalidOperationException("Follow relationship already exists.");
        }
    }
}
