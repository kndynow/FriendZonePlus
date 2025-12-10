using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.Services.Authentication;
using Moq;

namespace FriendZonePlus.UnitTests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordHelper> _passwordHelperMock;
        private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordHelperMock = new Mock<IPasswordHelper>();
            _jwtGeneratorMock = new Mock<IJwtGenerator>();
            _authenticationService = new AuthenticationService(
                _userRepoMock.Object,
                _passwordHelperMock.Object,
                _jwtGeneratorMock.Object);
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
        public async Task RegisterAsync_ShouldSaveUser_AndReturnAuthResponse()
        {
            // Arrange
            var registerRequest = new RegisterRequest(
                "Snusmumriken1978",
                "snusmumriken@hotmail.com",
                "Erik",
                "Eriksson",
                "Secret123"
            );

            _userRepoMock.Setup(r => r.GetByEmailAsync(registerRequest.Email))
                .ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.GetByUsernameAsync(registerRequest.Username))
                .ReturnsAsync((User?)null);

            _passwordHelperMock
                .Setup(h => h.HashPassword("Secret123"))
                .Returns("hashed-Secret123");

            // Repository returns user with ID assigned
            _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Callback((User u) => u.Id = 1)
                .Returns(Task.CompletedTask);

            _jwtGeneratorMock
                .Setup(j => j.GenerateToken(It.IsAny<User>()))
                .Returns("fake-jwt-token");

            // Act
            var result = await _authenticationService.RegisterAsync(registerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal("Snusmumriken1978", result.Username);

            _passwordHelperMock.Verify(h => h.HashPassword("Secret123"), Times.Once);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldHashPassword_BeforeSaving()
        {
            // Arrange
            var registerRequest = new RegisterRequest(
                "Snusmumriken1978",
                "snusmumriken@hotmail.com",
                "Erik",
                "Eriksson",
                "Secret123"
            );

            _userRepoMock.Setup(r => r.GetByEmailAsync(registerRequest.Email))
                .ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.GetByUsernameAsync(registerRequest.Username))
                .ReturnsAsync((User?)null);

            _passwordHelperMock
                .Setup(h => h.HashPassword(It.IsAny<string>()))
                .Returns((string pwd) => "hashed-" + pwd);

            _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Callback((User u) => u.Id = 5)
                .Returns(Task.CompletedTask);

            _jwtGeneratorMock
                .Setup(j => j.GenerateToken(It.IsAny<User>()))
                .Returns("fake-jwt-token");

            // Act
            await _authenticationService.RegisterAsync(registerRequest);

            // Assert: hashing happened
            _passwordHelperMock.Verify(h => h.HashPassword("Secret123"), Times.Once);

            // Assert: repository got the hashed value
            _userRepoMock.Verify(r =>
                r.AddAsync(It.Is<User>(u => u.PasswordHash == "hashed-Secret123")),
                Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var registerRequest = new RegisterRequest(
                "Snusmumriken1978",
                "snusmumriken@hotmail.com",
                "Erik",
                "Eriksson",
                "Secret123"
            );

            var existingUser = ValidUser();
            existingUser.Id = 1;

            _userRepoMock.Setup(r => r.GetByEmailAsync(registerRequest.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FriendZonePlus.Core.Exceptions.UserAlreadyExistsException>(
                async () => await _authenticationService.RegisterAsync(registerRequest));

            Assert.Contains(registerRequest.Email, exception.Message);

            _userRepoMock.Verify(r => r.GetByEmailAsync(registerRequest.Email), Times.Once);
            _userRepoMock.Verify(r => r.GetByUsernameAsync(It.IsAny<string>()), Times.Never);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
            _passwordHelperMock.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var registerRequest = new RegisterRequest(
                "Snusmumriken1978",
                "newemail@hotmail.com",
                "Erik",
                "Eriksson",
                "Secret123"
            );

            var existingUser = ValidUser();
            existingUser.Id = 1;

            _userRepoMock.Setup(r => r.GetByEmailAsync(registerRequest.Email))
                .ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.GetByUsernameAsync(registerRequest.Username))
                .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FriendZonePlus.Core.Exceptions.UserAlreadyExistsException>(
                async () => await _authenticationService.RegisterAsync(registerRequest));

            Assert.Contains(registerRequest.Username, exception.Message);

            _userRepoMock.Verify(r => r.GetByEmailAsync(registerRequest.Email), Times.Once);
            _userRepoMock.Verify(r => r.GetByUsernameAsync(registerRequest.Username), Times.Once);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
            _passwordHelperMock.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        #region Login Tests
        // === Login Tests ===
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = ValidUser();
            user.Id = 1;
            var loginRequest = new LoginRequest("Snusmumriken1978", "Secret123");
            var expectedToken = "fake-jwt-token";

            _userRepoMock
                .Setup(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail))
                .ReturnsAsync(user);

            _passwordHelperMock
                .Setup(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash))
                .Returns(true);

            _jwtGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var result = await _authenticationService.LoginAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result.Token);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);

            _userRepoMock.Verify(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail), Times.Once);
            _passwordHelperMock.Verify(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash), Times.Once);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldTryEmail_WhenUsernameIsNotFound()
        {
            // Arrange
            var user = ValidUser();
            user.Id = 1;
            var loginRequest = new LoginRequest("snusmumriken@hotmail.com", "Secret123");
            var expectedToken = "fake-jwt-token";

            _userRepoMock
                .Setup(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail))
                .ReturnsAsync(user);

            _passwordHelperMock
                .Setup(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash))
                .Returns(true);

            _jwtGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var result = await _authenticationService.LoginAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result.Token);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            _userRepoMock.Verify(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail), Times.Once);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnLoginResponse_WhenUsernameIsFound()
        {
            // Arrange
            var user = ValidUser();
            user.Id = 1;
            var loginRequest = new LoginRequest("Snusmumriken1978", "Secret123");
            var expectedToken = "fake-jwt-token";

            _userRepoMock
                .Setup(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail))
                .ReturnsAsync(user);

            _passwordHelperMock
                .Setup(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash))
                .Returns(true);

            _jwtGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var result = await _authenticationService.LoginAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result.Token);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            _userRepoMock.Verify(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var loginRequest = new LoginRequest("NoneExsistentUser", "Secret123");

            _userRepoMock
            .Setup(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail))
            .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<FriendZonePlus.Core.Exceptions.InvalidCredentialsException>(
                async () => await _authenticationService.LoginAsync(loginRequest));

            _userRepoMock.Verify(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail), Times.Once);
            _passwordHelperMock.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = ValidUser();
            var loginRequest = new LoginRequest("Snusmumriken1978", "WrongPassword123");

            _userRepoMock.Setup(r => r.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail))
            .ReturnsAsync(user);

            _passwordHelperMock.Setup(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash))
            .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<FriendZonePlus.Core.Exceptions.InvalidCredentialsException>(
                async () => await _authenticationService.LoginAsync(loginRequest));

            _passwordHelperMock.Verify(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash), Times.Once);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }
        #endregion
    }
}




