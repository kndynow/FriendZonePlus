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
  public async Task RegisterUser_ShouldCallRepository_WhenDataIsValid()
  {
    //Arrange
    var dto = new CreateUserDto("TestUser", "test@test.com");

    //If trying to create, return a User with ID 1
    _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(new User { Id = 1 });

    // Create service and send mock database


    //Act
    var result = await _sut.CreateUserAsync(dto);

    //Assert
    Assert.Equal(1, result);

    //Check if AddAsync was called one time
    _userRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
        u.Username == "TestUser" &&
        u.Email == "test@test.com"
    )), Times.Once);
  }

  [Fact]
  public async Task RegisterUser_ShouldThrowException_WhenUsernameIsEmpty()
  {
    //Arrange
    var invalidDto = new CreateUserDto("", "test@test.com");

    //Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateUserAsync(invalidDto));

    //Verify that database never was called
    _userRepoMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
  }

}
