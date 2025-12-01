using FriendZonePlus.Core.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendZonePlus.UnitTests.Services
{
    public class AuthorizationServiceTests
    {
        //[Fact]
        //public async Task CreateUser_ShouldReturnSuccess_WhenDataIsValid()
        //{
        //    //Arrange
        //    var dto = new CreateUserDto("TestUser", "test@test.com");

        //    //Simulate that database returns an ID
        //    _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
        //            .ReturnsAsync((User user) =>
        //            {
        //                user.Id = 1;
        //                return user;
        //            });
        //    //Act
        //    var userId = await _sut.CreateUserAsync(dto);

        //    //Assert
        //    Assert.Equal(1, userId);

        //    //Check if AddAsync was called one time with the correct user data
        //    _userRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
        //        u.Username == "TestUser" &&
        //        u.Email == "test@test.com"
        //    )), Times.Once);
        //}

        //[Fact]
        //public async Task CreateUser_ShouldThrowException_WhenUsernameIsEmpty()
        //{
        //    //Arrange
        //    var invalidDto = new CreateUserDto("", "test@test.com");

        //    //Act & Assert
        //    await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateUserAsync(invalidDto));

        //    //Verify that AddAsync was never called
        //    _userRepoMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        //}
    }
}
