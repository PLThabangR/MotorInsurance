namespace Shared.Abstractions.Results;

/// <summary>
/// Represents the result of an operation that can succeed or fail.
/// This is an alternative to throwing exceptions for expected failures.
/// 
/// Example use:
/// var result = Result.Success();
/// var failure = Result.Failure("Invalid premium amount");
/// </summary>
public class Result
{
    protected Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Gets whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error message if the operation failed.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result Success() => new(true, null);

    /// <summary>
    /// Creates a failure result with an error message.
    /// </summary>
    public static Result Failure(string error) => new(false, error);
}

/// <summary>
/// Represents the result of an operation with a value.
/// </summary>
/// <typeparam name="T">Type of the result value</typeparam>
/// 
/// Example use:
/// var result = Result<Quote>.Success(quote);
/// var failure = Result<Quote>.Failure("Quote not found");
public class Result<T> : Result
{
    private Result(bool isSuccess, string? error, T? value)
        : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the value if the operation succeeded.
    /// Will be null if the operation failed.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    public static Result<T> Success(T value) => new(true, null, value);

    /// <summary>
    /// Creates a failure result with an error message.
    /// </summary>
    public static new Result<T> Failure(string error) => new(false, error, default);
}