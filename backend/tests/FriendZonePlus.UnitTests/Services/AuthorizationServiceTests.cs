using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;
using FriendZonePlus.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendZonePlus.UnitTests.Services
{
    public class AuthorizationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly AuthorizationService _authorizationService;

        public AuthorizationServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _authorizationService = new AuthorizationService(_userRepoMock.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnResponseDto_WhenDataIsValid()
        {
            // Arrange
            var dto = new RegisterUserRequestDto(
                  "Snusmumriken1978",
                  "snusmumriken@hotmail.com",
                  "Erik",
                  "Eriksson",
                  "secret123"
                );

            // Simulate that database returns an ID
            _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                                .ReturnsAsync((User user) =>
                                {
                                    user.Id = 1;
                                    return user;
                                });
            //Act
            var result = await _authorizationService.CreateUserAsync(dto);

            //Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Snusmumriken1978", result.Username);
        }

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
