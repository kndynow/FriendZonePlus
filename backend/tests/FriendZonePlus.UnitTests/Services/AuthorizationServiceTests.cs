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
            var validator = new RegisterUserRequestDtoValidator(_userRepoMock.Object);
            _passwordHelperMock = new Mock<IPasswordHelper>(); 
            _authorizationService = new AuthorizationService(_userRepoMock.Object, _passwordHelperMock.Object, validator);
        }

        // Create a valid DTO
        private RegisterUserRequestDto ValidDto() =>
          new("Snusmumriken1978", "snusmumriken@hotmail.com", "Erik", "Eriksson", "Secret123");

        [Fact]
        public async Task CreateUserAsync_ShouldReturnResponseDto_WhenDataIsValid()
        {
            // Arrange
            var dto = ValidDto();

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

        [Fact]
        public async Task CreateUserAsync_ShouldCallPasswordHelperAndHashPassword()
        {
            // Arrange
            var dto = ValidDto();

            // Setting up the _passwordHelperMock so it returns a predictable hash
            _passwordHelperMock
            .Setup(h => h.HashPassword(It.IsAny<string>()))
            .Returns((string pwd) => "hashed-" + pwd);

            // Simulate that database returns an ID
            _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                                .ReturnsAsync((User user) =>
                                {
                                    user.Id = 1;
                                    return user;
                                });
            // Act
            await _authorizationService.CreateUserAsync(dto);

            // Assert
            _passwordHelperMock.Verify(h => h.HashPassword("Secret123"), Times.Once);
            _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.PasswordHash == "hashed-Secret123")), Times.Once);
        }

        [Fact]
        public async Task UserRepository_ShouldNotBeCalled_WhenValidationFails()
        {
            //Arrange
            var invalidDto = new RegisterUserRequestDto(
                  "",
                  "snusmumriken@hotmail.com",
                  "Erik",
                  "Eriksson",
                  "unsafepassword"
                );

            //Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authorizationService.CreateUserAsync(invalidDto));
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Theory]
        [InlineData("", "Username is required")]
        [InlineData("ab", "Username must be at least 3 characters")]
        [InlineData("a-very-long-username-exceeding-20", "Username cannot exceed 20 characters")]
        public async Task CreateUserAsync_ShouldThrowValidationException_ForInvalidUsername(string username, string expectedMessage)
        {
            // Arrange
            var invalidDto = ValidDto();
            invalidDto = invalidDto with { Username = username };

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _authorizationService.CreateUserAsync(invalidDto));
            // Assert
            Assert.Contains(expectedMessage, ex.Errors.Select(e => e.ErrorMessage));
        }

        [Theory]
        [InlineData("", "Email is required")]
        [InlineData("invalidEmail.com", "Invalid email")]
        [InlineData("ab", "Email must be longer than 5 characters")]
        [InlineData("extremelyLongEmailextremelyLongEmailextremelyLongEmailextremelyLongEmail", "Email cannot exceed 50 characters")]
        public async Task CreateUserAsync_ShouldThrowValidationException_ForInvalidEmail(string email, string expectedMessage)
        {
            // Arrange
            var invalidDto = ValidDto();
            invalidDto = invalidDto with { Email = email };

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _authorizationService.CreateUserAsync(invalidDto));

            // Assert
            Assert.Contains(expectedMessage, ex.Errors.Select(e => e.ErrorMessage));
        }

        [Theory]
        [InlineData("", "Password is required")]
        [InlineData("Ab3", "Password must be longer than 6 characters")]
        [InlineData("tooLongPasswordtooLongPasswordtooLongPassword", "Password cannot be longer than 30 characters")]
        [InlineData("aaaaaaa1", "Password must contain at least one uppercase letter")]
        [InlineData("AAAAAAA1", "Password must contain at least one lowercase letter")]
        [InlineData("AAAaaa", "Password must contain at least one number")]
        public async Task CreateUserAsync_ShouldThrowValidationException_ForInvalidPassword(string password, string expectedMessage)
        {
            // Arrange
            var invalidDto = ValidDto();
            invalidDto = invalidDto with { Password = password };

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _authorizationService.CreateUserAsync(invalidDto));

            // Assert
            Assert.Contains(expectedMessage, ex.Errors.Select(e => e.ErrorMessage));
        }

        [Theory]
        [InlineData("", "First name is required")]
        [InlineData("ThisNameIsWayTooLongToBeValidBecauseItExceedsThirtyCharacters", "First name cannot exceed 30 characters")]
        [InlineData("John123", "First name can only contain letters")]
        [InlineData("Jane-Doe", "First name can only contain letters")]
        public async Task CreateUserAsync_ShouldThrowValidationException_ForInvalidFirstName(string firstName, string expectedMessage)
        {
            // Arrange
            var invalidDto = ValidDto();
            invalidDto = invalidDto with { FirstName = firstName };

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _authorizationService.CreateUserAsync(invalidDto));

            // Assert
            Assert.Contains(expectedMessage, ex.Errors.Select(e => e.ErrorMessage));
        }

        [Theory]
        [InlineData("", "Last name is required")]
        [InlineData("ThisLastNameIsWayTooLongToBeValidBecauseItExceedsThirtyCharacters", "Last name cannot exceed 30 characters")]
        [InlineData("Smith123", "Last name can only contain letters and hyphens")]
        public async Task CreateUserAsync_ShouldThrowValidationException_ForInvalidLastName(string lastName, string expectedMessage)
        {
            // Arrange
            var invalidDto = ValidDto();
            invalidDto = invalidDto with { LastName = lastName };

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _authorizationService.CreateUserAsync(invalidDto));

            // Assert
            Assert.Contains(expectedMessage, ex.Errors.Select(e => e.ErrorMessage));
        }
    }
}
