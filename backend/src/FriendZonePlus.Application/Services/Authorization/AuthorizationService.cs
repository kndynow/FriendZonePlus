using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.DTOs;
using FluentValidation;
using FluentValidation.Results;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IValidator<RegisterUserRequestDto> _validator;

    public AuthorizationService(IUserRepository userRepository, IPasswordHelper passwordHelper, IValidator<RegisterUserRequestDto> validator)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _validator = validator;
    }

    public async Task<RegisterUserResponseDto> CreateUserAsync(RegisterUserRequestDto requestDto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(requestDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var hashedPassword = _passwordHelper.HashPassword(requestDto.Password);

        var user = new User
        {
            Username = requestDto.Username,
            Email = requestDto.Email,
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        var savedUser = await _userRepository.AddAsync(user);

        return new RegisterUserResponseDto(
            savedUser.Id,
            savedUser.Username,
            savedUser.CreatedAt
        );
    }
}