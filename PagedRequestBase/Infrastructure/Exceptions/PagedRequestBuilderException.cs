using System;

namespace PagedRequestBuilder.Infrastructure.Exceptions;
internal abstract class PagedRequestBuilderException : Exception
{
    public PagedRequestBuilderException() : base()
    {
    }

    public PagedRequestBuilderException(string message) : base(message)
    {
    }

    public PagedRequestBuilderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
