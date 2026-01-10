namespace ARAP.SharedKernel;

/// <summary>
/// Represents the result of an operation that can succeed or fail
/// </summary>
public class Result
{
    /// <summary>
    /// Gets whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; }
    
    /// <summary>
    /// Gets whether the operation failed
    /// </summary>
    public bool IsFailure => !IsSuccess;
    
    /// <summary>
    /// Gets the error message if the operation failed
    /// </summary>
    public string? Error { get; }

    protected Result(bool isSuccess, string? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("A successful result cannot have an error.");
        if (!isSuccess && error == null)
            throw new InvalidOperationException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static Result Success() => new(true, null);
    
    /// <summary>
    /// Creates a failed result with an error message
    /// </summary>
    public static Result Failure(string error) => new(false, error);

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);
    
    /// <summary>
    /// Creates a failed result with an error message
    /// </summary>
    public static Result<TValue> Failure<TValue>(string error) => new(default, false, error);
}

/// <summary>
/// Represents the result of an operation that returns a value
/// </summary>
public class Result<TValue> : Result
{
    /// <summary>
    /// Gets the value of the successful operation
    /// </summary>
    public TValue? Value { get; }

    protected internal Result(TValue? value, bool isSuccess, string? error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}
