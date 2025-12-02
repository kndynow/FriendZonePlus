using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Microsoft.VisualBasic.FileIO;
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
    //TODO: Get followed users

    public async Task UnfollowAsync(int followerId, int followedUserId)
    {
        await ValidateUnfollowRequest(followerId, followedUserId);

        var relation = await GetExistingFollowRelation(followerId, followedUserId);

        await _followRepository.DeleteAsync(relation);
    }

    private async Task ValidateFollowRequest(int followerId, int followedUserId)
    {
        ValidateIdShouldBeGreaterThanZero(followerId);
        ValidateIdShouldBeGreaterThanZero(followedUserId);
        ValidateSelfFollow(followerId, followedUserId);
        await ValidateFollowerExist(followerId);
        await ValidateFollowedUserExist(followedUserId);
        await ValidateUniqueFollow(followerId, followedUserId);
    }

    private async Task ValidateUnfollowRequest(int followerId, int followedUserId)
    {
        ValidateIdShouldBeGreaterThanZero(followerId);
        ValidateIdShouldBeGreaterThanZero(followedUserId);
        ValidateSelfFollow(followerId, followedUserId);
        await ValidateFollowerExist(followerId);
        await ValidateFollowedUserExist(followedUserId);
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

    private async Task ValidateFollowedUserExist(int followedUserId)
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

    private async Task<Follow> GetExistingFollowRelation(int followerId, int followedUserId)
    {
        var relation = await _followRepository.GetFollowRelationAsync(followerId, followedUserId);

        if (relation == null)
            throw new InvalidOperationException("Follow relationship does not exist.");

        return relation;
    }
}
