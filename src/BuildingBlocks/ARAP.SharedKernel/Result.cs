namespace ARAP.SharedKernel;


public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
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

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);
    public static Result<TValue> Failure<TValue>(string error) => new(default, false, error);
}


public class Result<TValue> : Result
{
    public TValue? Value { get; }

    protected internal Result(TValue? value, bool isSuccess, string? error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}
