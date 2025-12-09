using FluentValidation;

namespace FriendZonePlus.API.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Find the DTO argument by type instead of index
        T? argument = default;
        for (int i = 0; i < context.Arguments.Count; i++)
        {
            if (context.Arguments[i] is T foundArgument)
            {
                argument = foundArgument;
                break;
            }
        }

        if (argument == null)
        {
            // If no argument of type T found, continue without validation
            return await next(context);
        }

        //Validate
        var validationResult = await validator.ValidateAsync(argument);

        if (!validationResult.IsValid)
        {
            // Return 400 with detailed error information
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        // Continue to endpoint if validation is successful
        return await next(context);
    }
}
