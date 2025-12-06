using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace FriendZonePlus.Application.Interfaces
{
    public interface ITokenValidator
    {
        /// <summary>
        /// Validate a JWT token string and return the ClaimsPrincipal when valid, otherwise null.
        /// </summary>
        ClaimsPrincipal? Validate(string? token);
    }
}
