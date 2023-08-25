using Microsoft.EntityFrameworkCore;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;

namespace PagedRequestBuilder.Services;

public class ExampleService : IExampleService
{
    private readonly ExampleContext _context;
    private readonly IPagedQueryBuilder<Example> _queryBuilder;

    public ExampleService(ExampleContext context, IPagedQueryBuilder<Example> queryBuilder)
    {
        _context = context;
        _queryBuilder = queryBuilder;
    }

    public async Task<PagedResponse<ExampleDto>> GetPaged(GetPagedExampleRequest? request)
    {
        request ??= new();
        var query = _context.Set<Example>();
        var pagedQuery = _queryBuilder
            .BuildQuery(query, request, true, 1, 100)
            .Include(x => x.Inner)
            .Select(x => x.Map<Example, ExampleDto>());

        var data = await pagedQuery.ToListAsync();
        var total = await pagedQuery.CountAsync();

        return data.ToPagedResponse(request.Page, request.Size, total);
    }
}

public interface IExampleService
{
    Task<PagedResponse<ExampleDto>> GetPaged(GetPagedExampleRequest? request);
}
