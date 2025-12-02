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
        _followRepoMock.Verify(repo => repo.AddAsync(
            It.Is<Follow> (follow => 
            follow.FollowerId == followerId && 
            follow.FollowedUserId == followedUserId)), Times.Once);
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

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service
            .FollowAsync(followerId, followedUserId));
     
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
    public async Task FollowAsync_ShouldThrowException_WhenFolloweeDoesNotExist()
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
    public async Task UnfollowAsync_ShouldRemoveFollowRelation_WhenItExists()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(r => r.GetByIdAsync(followerId))
            .ReturnsAsync(new User());
        _userRepoMock.Setup(r => r.GetByIdAsync(followedUserId))
            .ReturnsAsync(new User());
        _followRepoMock.Setup(r => r.GetFollowRelationAsync(followerId, followedUserId))
            .ReturnsAsync(new Follow { FollowerId = followerId, FollowedUserId = followedUserId });

        // Act
        await _service.UnfollowAsync(followerId, followedUserId);

        // Assert
        _followRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Follow>()), Times.Once);
    }

    [Fact]
    public async Task UnfollowAsync_ShouldThrowException_WhenRelationDoesNotExist()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(r => r.GetByIdAsync(followerId)).ReturnsAsync(new User());
        _userRepoMock.Setup(r => r.GetByIdAsync(followedUserId)).ReturnsAsync(new User());

        _followRepoMock.Setup(r => r.GetFollowRelationAsync(followerId, followedUserId))
                       .ReturnsAsync((Follow?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task UnfollowAsync_ShouldThrowException_WhenFollowerDoesNotExist()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new User());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UnfollowAsync(1, 2));
    }

    [Fact]
    public async Task UnfollowAsync_ShouldThrowException_WhenFollowedUserDoesNotExist()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new User());
        _userRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 3)]
    [InlineData(1, 0)]
    [InlineData(4, -9)]
    public async Task UnfollowAsync_ShouldThrowException_WhenIdsAreInvalid(int followerId, int followedUserId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task UnfollowAsync_ShouldThrowException_WhenFollowerAndFollowedUserIdAreEqual()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));
    }
}