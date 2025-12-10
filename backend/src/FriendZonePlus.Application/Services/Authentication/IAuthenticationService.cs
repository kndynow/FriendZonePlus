using FriendZonePlus.Application.DTOs;

namespace FriendZonePlus.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}