namespace JamunaBank.Procurement.API.Models;

public sealed record ApiResponse<T>
{
    public required bool Success { get; init; }
    public required string Message { get; init; }
    public T? Data { get; init; }
    public IReadOnlyCollection<string> Errors { get; init; } = [];
    public required string TraceId { get; init; }

    public static ApiResponse<T> Ok(T? data, string message, string traceId) =>
        new() { Success = true, Message = message, Data = data, TraceId = traceId };

    public static ApiResponse<T> Fail(string message, IEnumerable<string> errors, string traceId) =>
        new() { Success = false, Message = message, Errors = [.. errors], TraceId = traceId };
}
