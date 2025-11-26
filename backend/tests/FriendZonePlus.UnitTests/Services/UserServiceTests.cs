using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;
using Xunit;


namespace FriendZonePlus.UnitTests.Services;

public class UserServiceTests
{
  [Fact]
  public async Task RegisterUser_ShouldCallRepository_WhenDataIsValid()
  {
    //Arrange

    var mockRepo = new Mock<IUserRepository>();

    //If trying to create, return a User with ID 1
    mockRepo.Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(new User { Id = 1 });

    // Create service and send mock database
    var service = new UserService(mockRepo.Object);
    var dto = new RegisterUserDto("TestUser", "test@test.com");

    //Act
    var result = await service.RegisterUserAsync(dto);

    //Assert
    //Should get ID 1 back
    Assert.Equal(1, result);

    //Check if AddAsync was called one time
    mockRepo.Verify(repo => repo.AddAsync(It.Is<User>(u =>
        u.Username == "TestUser" &&
        u.Email == "test@test.com"
    )), Times.Once);
  }

  [Fact]
  public async Task RegisterUser_ShouldThrowException_WhenUsernameIsEmpty()
  {
    //Arrange
    var mockRepo = new Mock<IUserRepository>();
    var service = new UserService(mockRepo.Object);

    var invalidDto = new RegisterUserDto("", "test@test.com");

    //Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterUserAsync(invalidDto));

    //Verify that database never was called
    mockRepo.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
  }

}
