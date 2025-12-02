namespace FriendZonePlus.Application.Helpers
{
    public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }
}