using FriendZonePlus.Application.DTOs;
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
  public async Task CreateUser_ShouldReturnSuccess_WhenDataIsValid()
  {
    //Arrange
    var dto = new CreateUserDto("TestUser", "test@test.com");

    //Simulate that database returns an ID
    _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User user) =>
            {
              user.Id = 1;
              return user;
            });
    //Act
    var userId = await _sut.CreateUserAsync(dto);

    //Assert
    Assert.Equal(1, userId);

    //Check if AddAsync was called one time with the correct user data
    _userRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
        u.Username == "TestUser" &&
        u.Email == "test@test.com"
    )), Times.Once);
  }

  [Fact]
  public async Task CreateUser_ShouldThrowException_WhenUsernameIsEmpty()
  {
    //Arrange
    var invalidDto = new CreateUserDto("", "test@test.com");

    //Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateUserAsync(invalidDto));

    //Verify that AddAsync was never called
    _userRepoMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
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
