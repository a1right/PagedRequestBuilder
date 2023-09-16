using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagedRequestBuilder.Infrastructure.AsyncSequence;

internal class AsyncEnumeratorWrapper<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _source;

    public AsyncEnumeratorWrapper(IEnumerator<T> source)
    {
        _source = source;
    }

    public T Current => _source.Current;

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.CompletedTask);
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_source.MoveNext());
    }
}
