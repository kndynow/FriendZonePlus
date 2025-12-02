public record RegisterUserRequestDto(string Username, string Email, string FirstName, string LastName, string Password);

public record RegisterUserResponseDto(int Id, string Username, DateTime CreatedAt);