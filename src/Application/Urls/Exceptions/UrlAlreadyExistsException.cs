namespace Shortener.Application.Urls.Exceptions;

public class UrlAlreadyExistsException : Exception
{
    public UrlAlreadyExistsException() : base("Url already exists.")
    {
    }

    public UrlAlreadyExistsException(string message)
        : base(message)
    {
    }

    public UrlAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}