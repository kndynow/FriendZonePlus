using FluentValidation;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Application.Validators;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.Services.Authentication;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using FriendZonePlus.Application.Helpers;

namespace FriendZonePlus.UnitTests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordHelper> _passwordHelperMock;
        private readonly Mock<IJwtHelper> _jwtHelperMock;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordHelperMock = new Mock<IPasswordHelper>();
            _jwtHelperMock = new Mock<IJwtHelper>();
            _authenticationService = new AuthenticationService(
                _userRepoMock.Object,
                _passwordHelperMock.Object,
                _jwtHelperMock.Object);
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
            var result = await _authenticationService.CreateUserAsync(user);

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
            await _authenticationService.CreateUserAsync(user);

            // Assert: hashing happened
            _passwordHelperMock.Verify(h => h.HashPassword("Secret123"), Times.Once);

            // Assert: repository got the hashed value
            _userRepoMock.Verify(r =>
                r.AddAsync(It.Is<User>(u => u.PasswordHash == "hashed-Secret123")),
                Times.Once);
        }


        #region Login Tests
        // === Login Tests ===
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = ValidUser();
            var username = "Snusmumriken1978";
            var password = "Secret123";
            var expectedToken = "fake-jwt-token";

            _userRepoMock
                .Setup(r => r.GetByUsernameAsync(username))
                .ReturnsAsync(user);

            _passwordHelperMock
                .Setup(h => h.VerifyPassword(password, user.PasswordHash))
                .Returns(true);

            _jwtHelperMock
                .Setup(j => j.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var result = await _authenticationService.LoginAsync(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result);

            _userRepoMock.Verify(r => r.GetByUsernameAsync(username), Times.Once);
            _passwordHelperMock.Verify(h => h.VerifyPassword(password, user.PasswordHash), Times.Once);
            _jwtHelperMock.Verify(j => j.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldTryEmail_WhenUsernameIsNotFound()
        {
            // Arrange
            var user = ValidUser();
            var email = "snusmumriken@hotmail.com";
            var password = "Secret123";
            var expectedToken = "fake-jwt-token";

            _userRepoMock
                .Setup(r => r.GetByUsernameAsync(email))
                .ReturnsAsync((User?)null);

            _userRepoMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);

            _passwordHelperMock
                .Setup(h => h.VerifyPassword(password, user.PasswordHash))
                .Returns(true);

            _jwtHelperMock
                .Setup(j => j.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var result = await _authenticationService.LoginAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result);
            _userRepoMock.Verify(r => r.GetByUsernameAsync(email), Times.Once);
            _userRepoMock.Verify(r => r.GetByEmailAsync(email), Times.Once);
            _jwtHelperMock.Verify(j => j.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldNotTryEmail_WhenUsernameIsFound()
        {
            // Arrange
            var user = ValidUser();
            var username = "Snusmumriken1978";
            var password = "Secret123";
            var expectedToken = "fake-jwt-token";

            _userRepoMock
                .Setup(r => r.GetByUsernameAsync(username))
                .ReturnsAsync(user);

            _passwordHelperMock
                .Setup(h => h.VerifyPassword(password, user.PasswordHash))
                .Returns(true);

            _jwtHelperMock
                .Setup(j => j.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var result = await _authenticationService.LoginAsync(username, password);

            // Assert
            Assert.NotNull(result);
            _userRepoMock.Verify(r => r.GetByUsernameAsync(username), Times.Once);
            _userRepoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var username = "NoneExsistentUser";
            var password = "Secret123";

            _userRepoMock
            .Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync((User?)null);

            _userRepoMock
            .Setup(r => r.GetByEmailAsync(username))
            .ReturnsAsync((User?)null);

            // Act
            var result = await _authenticationService.LoginAsync(username, password);

            // Assert
            Assert.Null(result);
            _userRepoMock.Verify(r => r.GetByUsernameAsync(username), Times.Once);
            _userRepoMock.Verify(r => r.GetByEmailAsync(username), Times.Once);
            _passwordHelperMock.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _jwtHelperMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = ValidUser();
            var username = "Snusmumriken1978";
            var wrongPassword = "WrongPassword123";

            _userRepoMock.Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync(user);

            _passwordHelperMock.Setup(h => h.VerifyPassword(wrongPassword, user.PasswordHash))
            .Returns(false);

            // Act
            var result = await _authenticationService.LoginAsync(username, wrongPassword);

            // Assert
            Assert.Null(result);
            _passwordHelperMock.Verify(h => h.VerifyPassword(wrongPassword, user.PasswordHash), Times.Once);
            _jwtHelperMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

    }
    #endregion
}




