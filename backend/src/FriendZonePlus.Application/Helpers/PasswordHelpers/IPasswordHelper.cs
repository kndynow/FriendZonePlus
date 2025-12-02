namespace FriendZonePlus.Application.Helpers.PasswordHelpers
{
    public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }
}