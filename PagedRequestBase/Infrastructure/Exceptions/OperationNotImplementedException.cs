using System;

namespace PagedRequestBuilder.Infrastructure.Exceptions;

internal class OperationNotImplementedException : PagedRequestBuilderException
{
    public OperationNotImplementedException() : base()
    {
    }

    public OperationNotImplementedException(string message) : base(message)
    {
    }

    public OperationNotImplementedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
