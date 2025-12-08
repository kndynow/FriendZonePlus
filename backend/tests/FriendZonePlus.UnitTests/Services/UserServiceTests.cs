using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Exceptions;
using FriendZonePlus.Application.Interfaces;
using Moq;
using Xunit;


namespace FriendZonePlus.UnitTests.Services;

public class UserServiceTests
{
  private readonly Mock<IUserRepository> _userRepoMock;
  private readonly UserService _sut;

  public UserServiceTests()
  {
    _userRepoMock = new Mock<IUserRepository>();
    _sut = new UserService(_userRepoMock.Object);
  }

  #region DeleteUserAsync Tests

  [Fact]
  public async Task DeleteUserAsync_ShouldDeleteUser_WhenUserExists()
  {
    //Arrange
    var userId = 1;
    var user = new User { Id = userId };
    _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

    //Act
    await _sut.DeleteUserAsync(userId);

    //Assert
    _userRepoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    _userRepoMock.Verify(repo => repo.DeleteAsync(It.Is<User>(u => u.Id == userId)), Times.Once);
  }

  [Fact]
  public async Task DeleteUserAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
  {
    //Arrange
    var userId = 1;
    _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

    //Act & Assert
    var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
      _sut.DeleteUserAsync(userId)
    );

    Assert.Equal($"User with ID {userId} not found", exception.Message);
    _userRepoMock.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Never);
  }

  #endregion

  #region GetUserProfileAsync Tests

  [Fact]
  public async Task GetUserProfileAsync_ShouldReturnUserProfileDto_WhenUserExists()
  {
    //Arrange
    var userId = 1;
    var user = new User
    {
      Id = userId,
      Username = "testuser",
      FirstName = "Test",
      LastName = "User",
      ProfilePictureUrl = "http://example.com/pic.jpg",
      Followers = new List<Follow> { new Follow(), new Follow() },
      Following = new List<Follow> { new Follow() }
    };
    _userRepoMock.Setup(repo => repo.GetByIdWithRelationsAsync(userId)).ReturnsAsync(user);

    //Act
    var result = await _sut.GetUserProfileAsync(userId);

    //Assert
    Assert.NotNull(result);
    Assert.Equal(userId, result.Id);
    Assert.Equal("testuser", result.Username);
    Assert.Equal("Test", result.FirstName);
    Assert.Equal("User", result.LastName);
    Assert.Equal("http://example.com/pic.jpg", result.ProfilePictureUrl);
    Assert.Equal(2, result.FollowersCount);
    Assert.Equal(1, result.FollowingCount);
    _userRepoMock.Verify(repo => repo.GetByIdWithRelationsAsync(userId), Times.Once);
  }

  [Fact]
  public async Task GetUserProfileAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
  {
    //Arrange
    var userId = 1;
    _userRepoMock.Setup(repo => repo.GetByIdWithRelationsAsync(userId)).ReturnsAsync((User?)null);

    //Act & Assert
    var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
      _sut.GetUserProfileAsync(userId)
    );

    Assert.Equal($"User with ID {userId} not found", exception.Message);
  }

  #endregion

  #region UpdateUserProfileAsync Tests

  [Fact]
  public async Task UpdateUserProfileAsync_ShouldUpdateUser_WhenUserExists()
  {
    //Arrange
    var userId = 1;
    var user = new User
    {
      Id = userId,
      FirstName = "Old",
      LastName = "Name",
      ProfilePictureUrl = "http://example.com/old.jpg"
    };
    var updateDto = new UpdateUserDto("New", "Name", "http://example.com/new.jpg");
    _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

    //Act
    await _sut.UpdateUserProfileAsync(userId, updateDto);

    //Assert
    Assert.Equal("New", user.FirstName);
    Assert.Equal("Name", user.LastName);
    Assert.Equal("http://example.com/new.jpg", user.ProfilePictureUrl);
    _userRepoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    _userRepoMock.Verify(repo => repo.UpdateAsync(It.Is<User>(u => u.Id == userId)), Times.Once);
  }

  [Fact]
  public async Task UpdateUserProfileAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
  {
    //Arrange
    var userId = 1;
    var updateDto = new UpdateUserDto("New", "Name", "http://example.com/new.jpg");
    _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

    //Act & Assert
    var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
      _sut.UpdateUserProfileAsync(userId, updateDto)
    );

    Assert.Equal($"User with ID {userId} not found", exception.Message);
    _userRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
  }

  #endregion

  #region FollowUserAsync Tests

  [Fact]
  public async Task FollowUserAsync_ShouldFollowUser_WhenUserExists()
  {
    //Arrange
    var currentUserId = 1;
    var targetUserId = 2;
    var targetUser = new User { Id = targetUserId };
    _userRepoMock.Setup(repo => repo.GetByIdAsync(targetUserId)).ReturnsAsync(targetUser);

    //Act
    await _sut.FollowUserAsync(currentUserId, targetUserId);

    //Assert
    _userRepoMock.Verify(repo => repo.GetByIdAsync(targetUserId), Times.Once);
    _userRepoMock.Verify(repo => repo.FollowUserAsync(currentUserId, targetUserId), Times.Once);
  }

  [Fact]
  public async Task FollowUserAsync_ShouldThrowCannotFollowSelfException_WhenTryingToFollowSelf()
  {
    //Arrange
    var userId = 1;

    //Act & Assert
    var exception = await Assert.ThrowsAsync<CannotFollowSelfException>(() =>
      _sut.FollowUserAsync(userId, userId)
    );

    Assert.Equal("You cannot follow yourself.", exception.Message);
    _userRepoMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
    _userRepoMock.Verify(repo => repo.FollowUserAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
  }

  [Fact]
  public async Task FollowUserAsync_ShouldThrowUserNotFoundException_WhenTargetUserDoesNotExist()
  {
    //Arrange
    var currentUserId = 1;
    var targetUserId = 2;
    _userRepoMock.Setup(repo => repo.GetByIdAsync(targetUserId)).ReturnsAsync((User?)null);

    //Act & Assert
    var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
      _sut.FollowUserAsync(currentUserId, targetUserId)
    );

    Assert.Equal($"User with ID {targetUserId} not found", exception.Message);
    _userRepoMock.Verify(repo => repo.FollowUserAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
  }

  #endregion

  #region UnfollowUserAsync Tests

  [Fact]
  public async Task UnfollowUserAsync_ShouldUnfollowUser_WhenUserExists()
  {
    //Arrange
    var currentUserId = 1;
    var targetUserId = 2;
    var targetUser = new User { Id = targetUserId };
    _userRepoMock.Setup(repo => repo.GetByIdAsync(targetUserId)).ReturnsAsync(targetUser);

    //Act
    await _sut.UnfollowUserAsync(currentUserId, targetUserId);

    //Assert
    _userRepoMock.Verify(repo => repo.GetByIdAsync(targetUserId), Times.Once);
    _userRepoMock.Verify(repo => repo.UnfollowUserAsync(currentUserId, targetUserId), Times.Once);
  }

  [Fact]
  public async Task UnfollowUserAsync_ShouldThrowUserNotFoundException_WhenTargetUserDoesNotExist()
  {
    //Arrange
    var currentUserId = 1;
    var targetUserId = 2;
    _userRepoMock.Setup(repo => repo.GetByIdAsync(targetUserId)).ReturnsAsync((User?)null);

    //Act & Assert
    var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
      _sut.UnfollowUserAsync(currentUserId, targetUserId)
    );

    Assert.Equal($"User with ID {targetUserId} not found", exception.Message);
    _userRepoMock.Verify(repo => repo.UnfollowUserAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
  }

  #endregion

}
