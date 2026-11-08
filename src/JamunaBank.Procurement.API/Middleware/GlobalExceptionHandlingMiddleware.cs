using System.Diagnostics;
using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Middleware;

public sealed class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger, IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception exception)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            var isValidationError = exception is ArgumentException;
            if (isValidationError)
                logger.LogWarning(exception, "API request validation failed. TraceId: {TraceId}", traceId);
            else
                logger.LogError(exception, "Unhandled API exception. TraceId: {TraceId}", traceId);
            context.Response.StatusCode = isValidationError
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(
                isValidationError
                    ? ApiResponse<object>.Fail("Validation failed.", [exception.Message], traceId)
                    : ApiResponse<object>.Fail(
                        "An unexpected error occurred.",
                        environment.IsDevelopment()
                            ? [$"{exception.GetBaseException().GetType().Name}: {exception.GetBaseException().Message}"]
                            : ["Contact support with the supplied trace identifier."],
                        traceId),
                context.RequestAborted);
        }
    }
}
