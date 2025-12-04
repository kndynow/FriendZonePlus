using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class FollowValidatorTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IFollowRepository> _followRepoMock;
    private readonly FollowValidator _validator;
    public FollowValidatorTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _followRepoMock = new Mock<IFollowRepository>();
        _validator = new FollowValidator(_userRepoMock.Object, _followRepoMock.Object);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(1, -2)]
    [InlineData(2, 0)]
    public async Task ValidateFollowAsync_ShouldThrowArgumentException_WhenFollowerIdOrFollowedUserIdIsInvalid(int followerId, int followedUserId)
    {
        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _validator.ValidateFollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateFollowAsync_ShouldThrowInvalidOperationException_WhenSelfFollow()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 1;

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateFollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateFollowAsync_ShouldThrowInvalidOperationException_WhenFollowerDoesNotExist()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync((User?)null);
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync(new User { Id = followedUserId });

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateFollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateFollowAsync_ShouldThrowInvalidOperationException_WhenFollowedUserDoesNotExist()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync(new User { Id = followerId });
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync((User?)null);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateFollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateFollowAsync_ShouldThrowInvalidOperationException_WhenFollowRelationAlreadyExists()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync(new User { Id = followerId });
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync(new User { Id = followedUserId });

        _followRepoMock.Setup(repo => repo.ExistsAsync(1, 2)).ReturnsAsync(true);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateFollowAsync(1, 2));
    }

    [Fact]
    public async Task ValidateFollowAsync_ShouldPass_WhenValidationsSucceed()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync(new User { Id = followerId });
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync(new User { Id = followedUserId });

        _followRepoMock.Setup(repo => repo.ExistsAsync(followerId, followedUserId)).ReturnsAsync(false);

        //Act & Assert
        await _validator.ValidateFollowAsync(followerId, followedUserId);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(1, -2)]
    [InlineData(2, 0)]
    public async Task ValidateUnfollowAsync_ShouldThrowArgumentException_WhenIdsAreInvalid(int followerId, int followedUserId)
    {
        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _validator.ValidateUnfollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateUnfollowAsync_ShouldThrowInvalidOperationException_WhenFollowRelationDoesNotExist()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync(new User { Id = followerId });
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync(new User { Id = followedUserId });

        _followRepoMock.Setup(repo => repo.ExistsAsync(followerId, followedUserId)).ReturnsAsync(false);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateUnfollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateUnfollowAsync_ShouldThrowInvalidOperationException_WhenSelfUnfollow()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 1;

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateUnfollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateUnfollowAsync_ShouldThrowInvalidOperationException_WhenFollowerDoesNotExist()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync((User?)null);
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync(new User { Id = followedUserId });

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateUnfollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateUnfollowAsync_ShouldThrowInvalidOperationException_WhenFollowedUserDoesNotExist()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync(new User { Id = followerId });
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync((User?)null);
        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _validator.ValidateUnfollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task ValidateUnfollowAsync_ShouldPass_WhenValidationsSucceed()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo.GetByIdAsync(followerId)).ReturnsAsync(new User { Id = followerId });
        _userRepoMock.Setup(repo => repo.GetByIdAsync(followedUserId)).ReturnsAsync(new User { Id = followedUserId });

        _followRepoMock.Setup(repo => repo.ExistsAsync(followerId, followedUserId)).ReturnsAsync(true);

        //Act & Assert
        await _validator.ValidateUnfollowAsync(followerId, followedUserId);
    }
}