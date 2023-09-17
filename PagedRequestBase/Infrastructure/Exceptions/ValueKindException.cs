using System.Text.Json;

namespace PagedRequestBuilder.Infrastructure.Exceptions;

internal class ValueKindException : PagedRequestBuilderException
{
    public ValueKindException(JsonValueKind valueKind) : base($"Value kind {valueKind} is not supported.")
    {

    }
}