using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class FollowServiceTests
{
    private readonly Mock<IFollowRepository> _followRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly FollowService _service;
    public FollowServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _followRepoMock = new Mock<IFollowRepository>();
        _service = new FollowService(_followRepoMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task FollowAsync_ShouldAddFollowRelation_WhenDataIsValid()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo
            .GetByIdAsync(followerId))
            .ReturnsAsync(new User { Id = followerId });
        _userRepoMock.Setup(repo => repo
            .GetByIdAsync(followedUserId))
            .ReturnsAsync(new User { Id = followedUserId });

        //Act
        await _service.FollowAsync(followerId, followedUserId);

        //Assert
        _followRepoMock.Verify(repo => repo.AddAsync(It.Is<Follow>(follow => follow.FollowerId == followerId && follow.FollowedUserId == followedUserId)), Times.Once);
    }

    [Fact]
    public async Task FollowAsync_ShouldThrowException_WhenFollowerAndFollowedUserIdAreEqual()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 1;
        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.FollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task FollowAsync_ShouldNotCallAddAsync_WhenFollowerAndFollowedUserIdAreEqual()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 1;
        //Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.FollowAsync(followerId, followedUserId));
        //Assert
        _followRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Follow>()), Times.Never);
    }

    [Fact]
    public async Task FollowAsync_ShouldThrowExeption_WhenFollowRelationAlreadyExists()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _followRepoMock.Setup(repo => repo
            .ExistsAsync(followerId, followedUserId))
            .ReturnsAsync(true);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.FollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task FollowAsync_ShouldThrowException_WhenFollowerDoesNotExist()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo
            .GetByIdAsync(followerId))
            .ReturnsAsync((User?)null);

        _userRepoMock.Setup(repo => repo
            .GetByIdAsync(followedUserId))
            .ReturnsAsync(new User { Id = followedUserId });

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.FollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task FollowAsync_ShouldThrowException_WhenFollowedUserDoesNotExist()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(repo => repo
            .GetByIdAsync(followerId))
            .ReturnsAsync(new User { Id = followerId });

        _userRepoMock.Setup(repo => repo
            .GetByIdAsync(followedUserId))
            .ReturnsAsync((User?)null);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.FollowAsync(followerId, followedUserId));
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, -3)]
    [InlineData(-1, 3)]
    [InlineData(9, 0)]
    public async Task FollowAsync_ShouldThrowException_WhenIdsAreInvalid(int followerId, int followedUserId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.FollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task UnfollowAsync_ShouldCallRemoveAsync_WhenFollowRelationExists()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _followRepoMock.Setup(repo => repo
                .ExistsAsync(followerId, followedUserId))
            .ReturnsAsync(true);

        // Act
        await _service.UnfollowAsync(followerId, followedUserId);

        // Assert
        _followRepoMock.Verify(repo =>
                repo.RemoveAsync(followerId, followedUserId),
            Times.Once);
    }

    [Fact]
    public async Task UnfollowAsync_ShouldThrowException_WhenFollowRelationDoesNotExist()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _followRepoMock.Setup(repo => repo
                .ExistsAsync(followerId, followedUserId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));

        _followRepoMock.Verify(repo =>
                repo.RemoveAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, -3)]
    [InlineData(-1, 3)]
    [InlineData(9, 0)]
    public async Task UnfollowAsync_ShouldThrowException_WhenIdsAreInvalid(int followerId, int followedUserId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));

        _followRepoMock.Verify(repo =>
                repo.RemoveAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task GetFollowersAsync_ShouldReturnFollowers_WhenUserExists()
    {
        // Arrange
        int userId = 1;
        var followerIds = new List<int> { 2, 3 };
        var expectedUsers = new List<User>
        {
            new() { Id = 2 },
            new() { Id = 3 }
        };

        _userRepoMock.Setup(repo => repo
                .GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });

        _followRepoMock.Setup(repo => repo
                .GetFollowerIdsAsync(userId))
            .ReturnsAsync(followerIds);

        _userRepoMock.Setup(repo => repo
                .GetByIdsAsync(followerIds))
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _service.GetFollowersAsync(userId);

        // Assert
        Assert.Same(expectedUsers, result);
    }

    [Fact]
    public async Task GetFollowersAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        int userId = 1;

        _userRepoMock.Setup(repo => repo
                .GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowersAsync(userId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetFollowersAsync_ShouldThrowException_WhenUserIdIsInvalid(int userId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowersAsync(userId));
    }

    [Fact]
    public async Task GetFollowedUsersAsync_ShouldReturnFollowedUsers_WhenUserExists()
    {
        // Arrange
        int userId = 1;
        var followedUserIds = new List<int> { 2, 3 };
        var expectedUsers = new List<User>
        {
            new() { Id = 2 },
            new() { Id = 3 }
        };

        _userRepoMock.Setup(repo => repo
                .GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId });

        _followRepoMock.Setup(repo => repo
                .GetFollowedUserIdsAsync(userId))
            .ReturnsAsync(followedUserIds);

        _userRepoMock.Setup(repo => repo
                .GetByIdsAsync(followedUserIds))
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _service.GetFollowedUsersAsync(userId);

        // Assert
        Assert.Same(expectedUsers, result);
    }

    [Fact]
    public async Task GetFollowedUsersAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        int userId = 1;

        _userRepoMock.Setup(repo => repo
                .GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowedUsersAsync(userId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetFollowedUsersAsync_ShouldThrowException_WhenUserIdIsInvalid(int userId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetFollowedUsersAsync(userId));
    }
}