using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;
using Xunit;

namespace FriendZonePlus.UnitTests.Services;

public class FollowServiceTests
{
    private readonly Mock<IFollowRepository> _followRepoMock;
    private readonly Mock<IFollowValidator> _validatorMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly FollowService _service;

    public FollowServiceTests()
    {
        _followRepoMock = new Mock<IFollowRepository>();
        _validatorMock = new Mock<IFollowValidator>();
        _userRepoMock = new Mock<IUserRepository>();
        _service = new FollowService(_followRepoMock.Object, _validatorMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task FollowAsync_ShouldCallValidatorAndAddFollow_WhenDataIsValid()
    {
        // Arrange
        var followerId = 1;
        var followedUserId = 2;

        _validatorMock
            .Setup(v => v.ValidateFollowAsync(followerId, followedUserId))
            .Returns(Task.CompletedTask);

        _followRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Follow>()))
            .ReturnsAsync((Follow f) => f);

        // Act
        var result = await _service.FollowAsync(followerId, followedUserId);

        // Assert
        _validatorMock.Verify(v => v.ValidateFollowAsync(followerId, followedUserId), Times.Once);
        _followRepoMock.Verify(r => r.AddAsync(It.Is<Follow>(f =>
            f.FollowerId == followerId && f.FollowedUserId == followedUserId)), Times.Once);
        Assert.Equal(followerId, result.FollowerId);
        Assert.Equal(followedUserId, result.FollowedUserId);
    }

    [Fact]
    public async Task FollowAsync_ShouldNotCallAddAsync_WhenValidatorThrows()
    {
        // Arrange
        var followerId = 1;
        var followedUserId = 1;

        _validatorMock
            .Setup(v => v.ValidateFollowAsync(followerId, followedUserId))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.FollowAsync(followerId, followedUserId));

        _followRepoMock.Verify(r => r.AddAsync(It.IsAny<Follow>()), Times.Never);
    }

    [Fact]
    public async Task UnfollowAsync_ShouldCallRemoveAsync_WhenFollowRelationExists()
    {
        // Arrange
        var followerId = 1;
        var followedUserId = 2;

        _followRepoMock
            .Setup(r => r.ExistsAsync(followerId, followedUserId))
            .ReturnsAsync(true);

        // Act
        await _service.UnfollowAsync(followerId, followedUserId);

        // Assert
        _followRepoMock.Verify(r => r.RemoveAsync(followerId, followedUserId), Times.Once);
    }

    [Fact]
    public async Task UnfollowAsync_ShouldThrow_WhenFollowRelationDoesNotExist()
    {
        // Arrange
        var followerId = 1;
        var followedUserId = 2;

        _followRepoMock
            .Setup(r => r.ExistsAsync(followerId, followedUserId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));

        _followRepoMock.Verify(r => r.RemoveAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 3)]
    [InlineData(9, 0)]
    public async Task UnfollowAsync_ShouldThrow_WhenIdsAreInvalid(int followerId, int followedUserId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));

        _followRepoMock.Verify(r => r.RemoveAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetFollowersAsync_ShouldReturnFollowers_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var followerIds = new List<int> { 2, 3 };
        var expectedUsers = new List<User>
        {
            new() { Id = 2 },
            new() { Id = 3 }
        };

        _userRepoMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });

        _followRepoMock
            .Setup(r => r.GetFollowerIdsAsync(userId))
            .ReturnsAsync(followerIds);

        _userRepoMock
            .Setup(r => r.GetByIdsAsync(followerIds))
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _service.GetFollowersAsync(userId);

        // Assert
        Assert.Same(expectedUsers, result);
    }

    [Fact]
    public async Task GetFollowersAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;

        _userRepoMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowersAsync(userId));

        _followRepoMock.Verify(r => r.GetFollowerIdsAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetFollowersAsync_ShouldThrow_WhenUserIdIsInvalid(int userId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowersAsync(userId));

        _userRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _followRepoMock.Verify(r => r.GetFollowerIdsAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetFollowedUsersAsync_ShouldReturnUsers_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var followedUserIds = new List<int> { 2, 3 };
        var expectedUsers = new List<User>
        {
            new() { Id = 2 },
            new() { Id = 3 }
        };

        _userRepoMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });

        _followRepoMock
            .Setup(r => r.GetFollowedUserIdsAsync(userId))
            .ReturnsAsync(followedUserIds);

        _userRepoMock
            .Setup(r => r.GetByIdsAsync(followedUserIds))
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _service.GetFollowedUsersAsync(userId);

        // Assert
        Assert.Same(expectedUsers, result);
    }

    [Fact]
    public async Task GetFollowedUsersAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;

        _userRepoMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowedUsersAsync(userId));

        _followRepoMock.Verify(r => r.GetFollowedUserIdsAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetFollowedUsersAsync_ShouldThrow_WhenUserIdIsInvalid(int userId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowedUsersAsync(userId));

        _userRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _followRepoMock.Verify(r => r.GetFollowedUserIdsAsync(It.IsAny<int>()), Times.Never);
    }
}