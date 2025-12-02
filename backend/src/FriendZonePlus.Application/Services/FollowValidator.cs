using FriendZonePlus.Core.Interfaces;

namespace FriendZonePlus.Application.Services
{
    public interface IFollowValidator
    {
        Task ValidateFollowAsync(int followerId, int followedUserId);
        Task ValidateUnfollowAsync(int followerId, int followedUserId);
    }

    public class FollowValidator : IFollowValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowRepository _followRepository;

        public FollowValidator(IUserRepository userRepository, IFollowRepository followRepository)
        {
            _userRepository = userRepository;
            _followRepository = followRepository;
        }

        public async Task ValidateFollowAsync(int followerId, int followedUserId)
        {
            ValidateUserId(followerId);
            ValidateUserId(followedUserId);
            ValidateSelfFollow(followerId, followedUserId);
            await ValidateFollowerExists(followerId);
            await ValidateFollowedUserExists(followedUserId);
            await ValidateUniqueFollow(followerId, followedUserId);
        }

        public async Task ValidateUnfollowAsync(int followerId, int followedUserId)
        {
            ValidateUserId(followerId);
            ValidateUserId(followedUserId);
            ValidateSelfFollow(followerId, followedUserId);
            await ValidateFollowerExists(followerId);
            await ValidateFollowedUserExists(followedUserId);
            await ValidateFollowRelationExists(followerId, followedUserId);
        }

        private void ValidateUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User Id must be greater than zero");
        }

        private void ValidateSelfFollow(int followerId, int followedUserId)
        {
            if (followerId == followedUserId)
                throw new InvalidOperationException("User cannot follow themselves.");
        }

        private async Task ValidateFollowerExists(int followerId)
        {
            var follower = await _userRepository.GetByIdAsync(followerId);

            if (follower == null)
                throw new InvalidOperationException("Follower does not exist.");
        }

        private async Task ValidateFollowedUserExists(int followedUserId)
        {
            var followedUser = await _userRepository.GetByIdAsync(followedUserId);

            if (followedUser == null)
                throw new InvalidOperationException("Followed user does not exist.");
        }

        private async Task ValidateUniqueFollow(int followerId, int followedUserId)
        {
            if (await _followRepository.ExistsAsync(followerId, followedUserId))
                throw new InvalidOperationException("Follow relationship already exists.");
        }

        private async Task ValidateFollowRelationExists(int followerId, int followedUserId)
        {
            if (!await _followRepository.ExistsAsync(followerId, followedUserId))
                throw new InvalidOperationException("Follow relationship does not exist.");
        }
    }
}
