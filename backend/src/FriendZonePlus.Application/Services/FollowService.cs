using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class FollowService
{
    private readonly IFollowRepository _followRepository;
    private readonly IFollowValidator _validator;
    private readonly IUserRepository _userRepository;

    public FollowService(
        IFollowRepository followRepository,
        IFollowValidator validator,
        IUserRepository userRepository)
    {
        _followRepository = followRepository;
        _validator = validator;
        _userRepository = userRepository;
    }

    // Follow user
    public async Task<Follow> FollowAsync(int followerId, int followedUserId)
    {
        await _validator.ValidateFollowAsync(followerId, followedUserId);

        var follow = new Follow
        {
            FollowerId = followerId,
            FollowedUserId = followedUserId
        };

        return await _followRepository.AddAsync(follow);
    }

    //Unfollow user
    public async Task UnfollowAsync(int followerId, int followedUserId)
    {
        await ValidateUnfollowRequest(followerId, followedUserId);
        await _followRepository.RemoveAsync(followerId, followedUserId);
    }

    private async Task ValidateUnfollowRequest(int followerId, int followedUserId)
    {
        ValidateIdShouldBeGreaterThanZero(followerId);
        ValidateIdShouldBeGreaterThanZero(followedUserId);
        await ValidateFollowRelationExists(followerId, followedUserId);
    }

    // Get followers
    public async Task<IEnumerable<User>> GetFollowersAsync(int userId)
    {
        ValidateIdShouldBeGreaterThanZero(userId);

        await ValidateUserExists(userId);

        var followerIds = await _followRepository.GetFollowerIdsAsync(userId);

        return await _userRepository.GetByIdsAsync(followerIds);
    }

    // Get followed users
    public async Task<IEnumerable<User>> GetFollowedUsersAsync(int userId)
    {
        ValidateIdShouldBeGreaterThanZero(userId);
        await ValidateUserExists(userId);

        var followedUserIds = await _followRepository.GetFollowedUserIdsAsync(userId);
        return await _userRepository.GetByIdsAsync(followedUserIds);
    }



    // Validation methods
    // Validate user exists
    private async Task ValidateUserExists(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} does not exist.");
        }
    }

    // Validate follow relationship exists
    private async Task ValidateFollowRelationExists(int followerId, int followedUserId)
    {
        if (!await _followRepository.ExistsAsync(followerId, followedUserId))
        {
            throw new InvalidOperationException("Follow relationship does not exist.");
        }
    }

    // Validate ID should be greater than zero
    private void ValidateIdShouldBeGreaterThanZero(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("User Id must be greater than zero");
        }
    }

    // Validate self follow
    private void ValidateSelfFollow(int followerId, int followedUserId)
    {
        if (followerId == followedUserId)
        {
            throw new InvalidOperationException("User cannot follow themselves.");
        }
    }

    // Validate follower exists
    private async Task ValidateFollowerExist(int followerId)
    {
        var follower = await _userRepository.GetByIdAsync(followerId);
        if (follower == null)
        {
            throw new InvalidOperationException("Follower does not exist.");
        }
    }

    // Validate followed user exists
    private async Task ValidateFollowedUserExist(int followedUserId)
    {
        var followedUser = await _userRepository.GetByIdAsync(followedUserId);
        if (followedUser == null)
        {
            throw new InvalidOperationException("Followed user does not exist.");
        }
    }

    // Validate unique follow
    private async Task ValidateUniqueFollow(int followerId, int followedUserId)
    {
        if (await _followRepository.ExistsAsync(followerId, followedUserId))
        {
            throw new InvalidOperationException("Follow relationship already exists.");
        }
    }
}
