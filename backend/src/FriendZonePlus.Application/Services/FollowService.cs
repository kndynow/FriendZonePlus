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

    public async Task FollowAsync(int followerId, int followedUserId)
    {
        
        await ValidateFollowRequest(followerId, followedUserId);

        var follow = new Follow
        {
            FollowerId = followerId,
            FollowedUserId = followedUserId
        };

        await _followRepository.AddAsync(follow);
    }

    //TODO: Get followers
    //TODO: Unfollow user

    private async Task ValidateFollowRequest(int followerId, int followedUserId)
    {
        ValidateIdShouldBeGreaterThanZero(followerId);
        ValidateIdShouldBeGreaterThanZero(followedUserId);
        ValidateSelfFollow(followerId, followedUserId);
        await ValidateFollowerExist(followerId);
        await ValidateFolloweeExist(followedUserId);
        await ValidateUniqueFollow(followerId, followedUserId);
    }

    private void ValidateIdShouldBeGreaterThanZero(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("User Id must be greater than zero");
        }
    }

    private void ValidateSelfFollow(int followerId, int followedUserId)
    {
        if (followerId == followedUserId)
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

    private async Task ValidateFolloweeExist(int followedUserId)
    {
        var followee = await _userRepository.GetByIdAsync(followedUserId);
        if (followee == null)
        {
            throw new InvalidOperationException("Followee does not exist.");
        }
    }

    private async Task ValidateUniqueFollow(int followerId, int followedUserId)
    {
        if (await _followRepository.ExistsAsync(followerId, followedUserId))
        {
            throw new InvalidOperationException("Follow relationship already exists.");
        }
    }
}
