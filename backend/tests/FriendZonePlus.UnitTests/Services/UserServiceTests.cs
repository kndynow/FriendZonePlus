using FriendZonePlus.API.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
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

  [Fact]
  public async Task DeleteUser_ShouldReturnSuccess_WhenUserExists()
  {
    //Arrange
    var userId = 1;
    //Simulate that database returns a user with the correct ID
    _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(new User { Id = userId });

    //Act
    var success = await _sut.DeleteUserAsync(userId);

    //Assert
    Assert.True(success);

    //Verify that DeleteAsync was called one time with the correct user data
    _userRepoMock.Verify(repo => repo.DeleteAsync(It.Is<User>(u => u.Id == userId)), Times.Once);
  }

}
