

namespace FriendZonePlus.Core.Exceptions;

public class UserAlreadyExistsException(string message) : Exception(message);
public class UserNotFoundException(string message) : Exception(message);
public class InvalidCredentialsException(string message) : Exception(message);


