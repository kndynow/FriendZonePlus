using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Exceptions;
using Mapster;

namespace FriendZonePlus.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IJwtGenerator _jwtGenerator;

    public AuthenticationService(IUserRepository userRepository, IPasswordHelper passwordHelper,
    IJwtGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) is not null)
        {
            throw new UserAlreadyExistsException($"Email {request.Email} already exists");
        }
        if (await _userRepository.GetByUsernameAsync(request.Username) is not null)
        {
            throw new UserAlreadyExistsException($"Username {request.Username} already exists");
        }

        var user = request.Adapt<User>();

        user.PasswordHash = _passwordHelper.HashPassword(request.Password);

        await _userRepository.AddAsync(user);
        var token = _jwtGenerator.GenerateToken(user);

        return new AuthResponse(token, user.Id, user.Username);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameOrEmailAsync(request.UsernameOrEmail);

        if (user is null)
        {
            throw new InvalidCredentialsException($"Invalid username or email: {request.UsernameOrEmail}");
        }
        if (!_passwordHelper.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException($"Invalid password for user: {user.Username}");
        }

        var token = _jwtGenerator.GenerateToken(user);

        return new LoginResponse(token, user.Id, user.Username, user.Email, user.FirstName, user.LastName);
    }
}