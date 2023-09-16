using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PagedRequestBuilder.Infrastructure.AsyncSequence;

internal static class AsyncEnumerable
{
    public static IAsyncEnumerable<T> Empty<T>() => EmptyAsyncEnumerator<T>.Instance;

    private class EmptyAsyncEnumerator<T> : IAsyncEnumerator<T>, IAsyncEnumerable<T>
    {
        public static readonly EmptyAsyncEnumerator<T> Instance = new EmptyAsyncEnumerator<T>();
        public T Current => default;
        public ValueTask DisposeAsync() => default;
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return this;
        }
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(false);
    }
}
