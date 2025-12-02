using FluentValidation.Results;
using System.Linq;

namespace FriendZonePlus.Application.Helpers.ValidationHelpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Converts a collection of FluentValidation failures into a dictionary
        /// grouped by property name (camelCase) and containing all error messages.
        /// 
        /// Example output:
        /// {
        ///     "username": ["Username is required", "Username must be at least 3 characters"],
        ///     "email": ["Email is required"]
        /// }
        /// </summary>

        public static object ToCamelCaseErrors(IEnumerable<ValidationFailure> failures)
        {
            return failures
                .GroupBy(e => ToCamelCase(e.PropertyName))
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }

        // Converts a string to camelCase by lowercasing the first character.
        // Example: "FirstName" -> "firstName"
        private static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || char.IsLower(s[0]))
                return s;

            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
    }
}