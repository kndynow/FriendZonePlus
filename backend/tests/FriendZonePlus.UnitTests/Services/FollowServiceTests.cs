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
    public async Task FollowAsync_ShouldThrowException_WhenFollowerAndFolloweeIdAreEqual()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 1;
        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.FollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task FollowAsync_ShouldNotCallAddAsync_WhenFollowerAndFolloweeIdAreEqual()
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
}