namespace ASecretCouncil.Host.Shared.Exceptions;

public class ApplicationException: Exception
{
    public ApplicationException(string? message) : base(message)
    {
    }
}
