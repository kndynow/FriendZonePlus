
using FluentValidation;

namespace FriendZonePlus.API.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Get DTO-Object from request
        var argument = context.GetArgument<T>(0);

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
