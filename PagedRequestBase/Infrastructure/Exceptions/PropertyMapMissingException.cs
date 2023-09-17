using System;

namespace PagedRequestBuilder.Infrastructure.Exceptions;

internal class PropertyMapMissingException : PagedRequestBuilderException
{
    public PropertyMapMissingException() : base()
    {
    }

    public PropertyMapMissingException(string message) : base(message)
    {
    }

    public PropertyMapMissingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
