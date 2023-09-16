using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace PagedRequestBuilder.Infrastructure.AsyncSequence;

internal class AsyncQueryable<T> : IAsyncEnumerable<T>, IQueryable<T>
{
    private IQueryable<T> _source;

    public AsyncQueryable(IQueryable<T> source)
    {
        _source = source;
    }

    public Type ElementType => typeof(T);

    public Expression Expression => _source.Expression;

    public IQueryProvider Provider => new AsyncQueryProvider<T>(_source.Provider);

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new AsyncEnumeratorWrapper<T>(_source.GetEnumerator());
    }

    public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
