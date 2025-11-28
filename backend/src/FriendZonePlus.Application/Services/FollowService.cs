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
        
        await ValidateFollowRequest(followerId, followeeId);

        var follow = new Follows
        {
            FollowerId = followerId,
            FolloweeId = followeeId
        };

        await _followRepository.AddAsync(follow);
    }

    //TODO: Get followers
    //TODO: Unfollow user

    private async Task ValidateFollowRequest(int followerId, int followeeId)
    {
        ValidateSelfFollow(followerId, followeeId);
        await ValidateFollowerExist(followerId);
        await ValidateFolloweeExist(followeeId);
        await ValidateUniqueFollow(followerId, followeeId);
    }

    private void ValidateSelfFollow(int followerId, int followeeId)
    {
        if (followerId == followeeId)
        {
            throw new InvalidOperationException("User cannot follow themselves.");
        }
    }

    private async Task ValidateFollowerExist(int followerId)
    {
        var follower = await _userRepository.GetByIdAsync(followerId);
        if (follower == null)
        {
            throw new InvalidOperationException("Follower does not exist.");
        }
    }

    private async Task ValidateFolloweeExist(int followeeId)
    {
        var followee = await _userRepository.GetByIdAsync(followeeId);
        if (followee == null)
        {
            throw new InvalidOperationException("Followee does not exist.");
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
