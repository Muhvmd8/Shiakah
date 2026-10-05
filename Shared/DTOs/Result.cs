namespace Shared.DTOs;
public class Result<T>
{
    public bool Succeeded { get; private set; }
    public string Message { get; private set; } = default!;
    public T? Data { get; private set; }

    public static Result<T> Success(T data, string message)
        => new() { Succeeded = true, Data = data, Message = message };

    public static Result<T> Fail(string message)
        => new() { Succeeded = false, Data = default, Message = message };
}

public class Result
{
    public bool Succeeded { get; private set; }
    public string Message { get; private set; } = default!;

    public static Result Success(string message)
        => new() { Succeeded = true, Message = message };

    public static Result Fail(string message)
        => new() { Succeeded = false, Message = message };
}
