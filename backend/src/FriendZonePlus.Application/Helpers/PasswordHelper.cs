using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace FriendZonePlus.Application.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
