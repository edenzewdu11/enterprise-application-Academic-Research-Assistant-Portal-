namespace ARAP.SharedKernel;

/// <summary>
/// Base exception class for domain-specific errors
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Initializes new instance of DomainException
    /// </summary>
    public DomainException()
    {
    }

    /// <summary>
    /// Initializes new instance with error message
    /// </summary>
    public DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes new instance with message and inner exception
    /// </summary>
    public DomainException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
