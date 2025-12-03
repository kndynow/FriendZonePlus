using FluentValidation;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Application.Validators;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Application.Helpers.PasswordHelpers;

using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendZonePlus.UnitTests.Services
{
    public class AuthorizationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordHelper> _passwordHelperMock;
        private readonly AuthorizationService _authorizationService;

        public AuthorizationServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordHelperMock = new Mock<IPasswordHelper>(); 
            _authorizationService = new AuthorizationService(_userRepoMock.Object, _passwordHelperMock.Object);
        }

        // Create a valid User
        private User ValidUser() =>
            new User
            {
                Username = "Snusmumriken1978",
                Email = "snusmumriken@hotmail.com",
                FirstName = "Erik",
                LastName = "Eriksson",
                PasswordHash = "Secret123", // plain password before hashing
                CreatedAt = DateTime.UtcNow
            };

        [Fact]
        public async Task CreateUserAsync_ShouldSaveUser_AndReturnSavedUser()
        {
            // Arrange
            var user = ValidUser();

            _passwordHelperMock
                .Setup(h => h.HashPassword("Secret123"))
                .Returns("hashed-Secret123");

            // Repository returns user with ID assigned
            _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) =>
                {
                    u.Id = 1;
                    return u;
                });

            // Act
            var result = await _authorizationService.CreateUserAsync(user);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Snusmumriken1978", result.Username);
            Assert.Equal("hashed-Secret123", result.PasswordHash);

            _passwordHelperMock.Verify(h => h.HashPassword("Secret123"), Times.Once);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldHashPassword_BeforeSaving()
        {
            // Arrange
            var user = ValidUser();

            _passwordHelperMock
                .Setup(h => h.HashPassword(It.IsAny<string>()))
                .Returns((string pwd) => "hashed-" + pwd);

            _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) =>
                {
                    u.Id = 5;
                    return u;
                });

            // Act
            await _authorizationService.CreateUserAsync(user);

            // Assert: hashing happened
            _passwordHelperMock.Verify(h => h.HashPassword("Secret123"), Times.Once);

            // Assert: repository got the hashed value
            _userRepoMock.Verify(r =>
                r.AddAsync(It.Is<User>(u => u.PasswordHash == "hashed-Secret123")),
                Times.Once);
        }
    }
}

