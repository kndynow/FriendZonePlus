using FriendZonePlus.Application.Helpers;
using Xunit;

namespace FriendZonePlus.UnitTests.Helpers;


public class PasswordHelperTests
{
    private readonly PasswordHelper _passwordHelper;

    public PasswordHelperTests()
    {
        _passwordHelper = new PasswordHelper();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        //Arrange
        var password = "secret123";

        //Act
        var hashedPassword = _passwordHelper.HashPassword(password);

        //Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }
    [Fact]
    public void VerifyPassword_ShouldReturnCorrectPassword()
    {
        //Arrange
        var password = "secret123";
        var hashedPassword = _passwordHelper.HashPassword(password);

        //Act
        var result = _passwordHelper.VerifyPassword(password, hashedPassword);

        //Assert
        Assert.True(result);
    }
    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        //Arrange
        var password = "secret123";
        var hashedPassword = _passwordHelper.HashPassword(password);

        //Act
        var result = _passwordHelper.VerifyPassword("incorrectPassword", hashedPassword);

        //Assert
        Assert.False(result);
    }
}
