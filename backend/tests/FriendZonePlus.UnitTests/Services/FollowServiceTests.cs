using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class FollowServiceTests
{
    private readonly Mock<IFollowRepository> _followRepoMock;
    private readonly Mock<IFollowValidator> _validatorMock;
    private readonly FollowService _service;
    public FollowServiceTests()
    {
        _followRepoMock = new Mock<IFollowRepository>();
        _validatorMock = new Mock<IFollowValidator>();
        _service = new FollowService(_followRepoMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task FollowAsync_ShouldAddFollowRelation_WhenDataIsValid()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 2;

        _validatorMock.Setup(v => v.ValidateFollowAsync(followerId, followedUserId))
            .Returns(Task.CompletedTask);

        //Act
        await _service.FollowAsync(followerId, followedUserId);

        //Assert
        _followRepoMock.Verify(repo => repo.AddAsync(
            It.Is<Follow> (follow => 
            follow.FollowerId == followerId && 
            follow.FollowedUserId == followedUserId)), Times.Once);
    }

    [Fact]
    public async Task FollowAsync_ShouldThrowException_WhenValidatorThrows()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 1;

        _validatorMock
            .Setup(v => v.ValidateFollowAsync(followerId, followedUserId))
            .ThrowsAsync(new InvalidOperationException());

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.FollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task FollowAsync_ShouldNotCallAddAsync_WhenValidatorThrows()
    {
        //Arrange
        int followerId = 1;
        int followedUserId = 1;

        _validatorMock
            .Setup(v => v.ValidateFollowAsync(followerId, followedUserId))
            .ThrowsAsync(new InvalidOperationException());

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

        _validatorMock
            .Setup(v => v.ValidateFollowAsync(followerId, followedUserId))
            .ThrowsAsync(new InvalidOperationException());

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
        // Arrange
        _validatorMock
            .Setup(v => v.ValidateFollowAsync(followerId, followedUserId))
            .ThrowsAsync(new ArgumentException());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.FollowAsync(followerId, followedUserId));
    }

    [Fact]
    public async Task UnfollowAsync_ShouldRemoveFollowRelation_WhenValidatorPasses()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _validatorMock
            .Setup(v => v.ValidateUnfollowAsync(followerId, followedUserId))
            .Returns(Task.CompletedTask);

        var relation = new Follow { FollowerId = followerId, FollowedUserId = followedUserId };

        _followRepoMock
            .Setup(r => r.GetFollowRelationAsync(followerId, followedUserId))
            .ReturnsAsync(relation);

        // Act
        await _service.UnfollowAsync(followerId, followedUserId);

        // Assert
        _followRepoMock.Verify(r => r.DeleteAsync(relation), Times.Once);
    }

    [Fact]
    public async Task UnfollowAsync_ShouldnotCallDeleteAsync_WhenRelationDoesNotExist()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _validatorMock
            .Setup(v => v.ValidateUnfollowAsync(followerId, followedUserId))
            .ThrowsAsync(new InvalidOperationException("Follow relationship does not exist."));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));

        _followRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Follow>()), Times.Never);
    }

    [Fact]
    public async Task UnfollowAsync_ShouldThrowException_WhenValidatorThrows()
    {
        // Arrange
        int followerId = 1;
        int followedUserId = 2;

        _validatorMock
            .Setup(v => v.ValidateUnfollowAsync(followerId, followedUserId))
            .ThrowsAsync(new InvalidOperationException());


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
        // Arrange
        _validatorMock
            .Setup(v => v.ValidateUnfollowAsync(followerId, followedUserId))
            .ThrowsAsync(new ArgumentException());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UnfollowAsync(followerId, followedUserId));
    }
}