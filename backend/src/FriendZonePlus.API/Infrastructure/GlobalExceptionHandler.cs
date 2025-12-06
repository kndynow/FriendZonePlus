using System;
using FriendZonePlus.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FriendZonePlus.API.Infrastructure;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
     ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An error occured: {Message}", exception.Message);

        // Map specific exceptions to HTTP status codes
        var (statusCode, title) = exception switch
        {
            ApplicationException => (StatusCodes.Status400BadRequest, "Bad request"),
            UserAlreadyExistsException => (StatusCodes.Status400BadRequest, "User already exists"),
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "Invalid credentials"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
        };

        problemDetails.Extensions.TryAdd("traceId", httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
