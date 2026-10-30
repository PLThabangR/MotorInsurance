using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models;

/// <summary>
/// Minimal result type used at Application boundaries where an operation can
/// fail with a set of user-facing error messages.
/// Kept deliberately small — not a full functional Result library.
/// </summary>
public class Result
{

    public bool Succeeded { get; private set; }
    public IReadOnlyList<string> Errors { get; }

    protected Result(bool succeeded, IReadOnlyList<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public static Result Ok()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Fail(params string[] errors)
    {
        return new Result(false, errors);
    }

    public static Result Fail(IEnumerable<string> errors)
    {   
        return new Result(false, errors.ToArray());
    }

    public void EnsureSucceeded(string contextMessage)
    {
        if (!Succeeded)
        {
            throw new ValidationException(string.IsNullOrWhiteSpace(contextMessage) ? string.Join("; ", Errors) : $"{contextMessage}:{string.Join("; ", Errors)}");
        }
    }
}// end of Result class


//Generic version of the Result class that allows for returning a value of type T along with the success/failure status and error messages. It inherits from the base Result class
//And adds a Value property to hold the result value when the operation is successful.

public class Result<T> : Result
{
    //Value returned here is the actual data stored inside your Result<T>
    public T? Value { get; }
    //The constructor receives a parameter called value and assigns it to the Value property:
    protected Result(bool ok, T? value, IReadOnlyList<string> errors)
        : base(ok, errors)
    {
        Value = value;
    }
    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, value, Array.Empty<string>());
    }
    public static new Result<T> Fail(params string[] errors)
    {
        return new Result<T>(false, default, errors);
    }
    public static new Result<T> Fail(IEnumerable<string> errors)
    {
        return new Result<T>(false, default, errors.ToArray());
    }

    // <summary>
    /// Checks whether the operation succeeded and returns its value.
    ///
    /// First, the base EnsureSucceeded() method checks the result.
    /// If the operation failed, it throws ValidationException.
    ///
    /// If successful, the stored Value is returned.
    ///
    /// Example:
    /// string userId = result.EnsureSucceeded("Registration failed");
    /// </summary>
    /// <param name="contextMessage">Description of the operation.</param>
    /// <returns>The value produced by the successful operation.</returns>
    public T EnsureSucceeded(string contextMessage)
    {
        base.EnsureSucceeded(contextMessage);
        return Value!;
    }
}
