namespace Shared.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; } = default(T?);
        public string? Message { get; init; }
        public IEnumerable<string>? Errors { get; init; }
        public object? Meta { get; init; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null, object? meta = null) =>
            new() { Success = true, Data = data, Message = message, Meta = meta };

        public static ApiResponse<T> Failure(string message, IEnumerable<string>? errors = null, int? unused = null) =>
            new() { Success = false, Message = message, Errors = errors };
    }
}
