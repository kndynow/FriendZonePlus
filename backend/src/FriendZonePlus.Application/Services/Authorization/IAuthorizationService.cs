
public interface IAuthorizationService
{
    Task<RegisterUserResponseDto> CreateUserAsync(RegisterUserRequestDto requestDto);
}