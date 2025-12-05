using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}