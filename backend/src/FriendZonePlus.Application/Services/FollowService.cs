using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services;

public class FollowService
{
    private readonly IFollowRepository _followRepository;
    private readonly IFollowValidator _validator;

    public FollowService(IFollowRepository followRepository, IFollowValidator validator)
    {
        _followRepository = followRepository;
        _validator = validator;
    }

    public async Task FollowAsync(int followerId, int followedUserId)
    {
        await _validator.ValidateFollowAsync(followerId, followedUserId);

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
        await _validator.ValidateUnfollowAsync(followerId, followedUserId);

        var relation = await GetExistingFollowRelation(followerId, followedUserId);

        await _followRepository.DeleteAsync(relation!);
    }

    private Task<Follow?> GetExistingFollowRelation(int followerId, int followedUserId)
    {
        return _followRepository.GetFollowRelationAsync(followerId, followedUserId);
    }
}