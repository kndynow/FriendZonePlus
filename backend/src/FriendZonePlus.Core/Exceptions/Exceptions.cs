

namespace FriendZonePlus.Core.Exceptions;

// Auth Exceptions
public class UserAlreadyExistsException(string message) : Exception(message);
public class UserNotFoundException(string message) : Exception(message);
public class InvalidCredentialsException(string message) : Exception(message);

// Follow Exceptions
public class CannotFollowSelfException() : Exception("You cannot follow yourself.");

// WallPost Exceptions
public class PostNotFoundException(int id) : Exception($"Post with id {id} was not found.");
public class UnauthorizedPostAccessException() : Exception("You are not authorized to perform this action on the post.");