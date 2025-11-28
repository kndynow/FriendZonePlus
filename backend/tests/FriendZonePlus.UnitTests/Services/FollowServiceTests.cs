using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class FollowServiceTests
{
    private readonly Mock<IFollowRepository> _followRepoMock;
    private readonly FollowService _service;
    public FollowServiceTests()
    {
        _followRepoMock = new Mock<IFollowRepository>();
        _service = new FollowService(_followRepoMock.Object);
    }

    [Fact]
    public async Task FollowAsync_ShouldAddFollowRelation_WhenDataIsValid()
    {
        //Arrange
        int followerId = 1;
        int followeeId = 2;

        //Act
        await _service.FollowAsync(followerId, followeeId);

        //Assert
        _followRepoMock.Verify(repo => repo.AddAsync(It.Is<Follows>(follow => follow.FollowerId == followerId && follow.FolloweeId == followeeId)), Times.Once);
    }

    [Fact]
    public async Task FollowAsync_ShouldThrowException_WhenFollowerAndFolloweeIdAreEqual()
    {
        //Arrange
        int followerId = 1;
        int followeeId = 1;
        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.FollowAsync(followerId, followeeId));
    }

    [Fact]
    public async Task FollowAsync_ShouldNotCallAddAsync_WhenFollowerAndFolloweeIdAreEqual()
    {
        //Arrange
        int followerId = 1;
        int followeeId = 1;
        //Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.FollowAsync(followerId, followeeId));
        //Assert
        _followRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Follows>()), Times.Never);
    }

    [Fact]
    public async Task FollowAsync_ShouldThrowExeption_WhenFollowRelationAlreadyExists()
    {
        //Arrange
        int followerId = 1;
        int followeeId = 2;

        _followRepoMock.Setup(repo => repo
            .ExistsAsync(followerId, followeeId))
            .ReturnsAsync(true);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.FollowAsync(followerId, followeeId));
    }
}