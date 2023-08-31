using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using TestApp.Models.Elasticsearch;
using TestApp.Models.MongoDriver;
using Example = PagedRequestBuilder.Models.Example;

namespace PagedRequestBuilder.Services;

public class ExampleService : IExampleService
{
    private readonly ExampleContext _context;
    private readonly ExampleMongoContext _exampleMongoContext;
    private readonly IPagedQueryBuilder<Example> _queryBuilder;
    private readonly IPagedQueryBuilder<ExampleDocument> _queryMongoBuilder;
    private readonly ElasticsearchClient _elastic;

    public ExampleService(
        ExampleContext context,
        ExampleMongoContext exampleMongoContext,
        IPagedQueryBuilder<Example> queryBuilder,
        IPagedQueryBuilder<ExampleDocument> queryMongoBuilder,
        ElasticsearchClient client)
    {
        _context = context;
        _exampleMongoContext = exampleMongoContext;
        _queryBuilder = queryBuilder;
        _queryMongoBuilder = queryMongoBuilder;
        _elastic = client;
    }

    public async Task<PagedResponse<ExampleDto>> GetPaged(GetPagedExampleRequest? request)
    {
        request ??= new();
        var query = _context.Set<Example>();
        var pagedQuery = _queryBuilder
            .BuildQuery(query, request, 1, 100)
            .Include(x => x.Inner)
            .ThenInclude(x => x!.Nested)
            .Select(x => x.Map<Example, ExampleDto>());

        Console.WriteLine(query.ToQueryString());

        var data = await pagedQuery.ToListAsync();
        var total = await pagedQuery.CountAsync();

        return data.ToPagedResponse(request.Page, request.Size, total);
    }

    public async Task<PagedResponse<ExampleDocument>> GetPaged(GetPagedExampleDocument? request)
    {
        request ??= new();

        var query = _exampleMongoContext.Examples;
        query = _queryMongoBuilder.BuildQuery(query, request);
        var data = await (await query.ToCursorAsync()).ToListAsync();
        var total = await query.CountAsync();
        return data.ToPagedResponse(request.Page, request.Size, total);
    }

    public IReadOnlyCollection<Example> GetPaged(GetPagedExampleElastic? request)
    {
        request ??= new();

        var getDescriptor = new GetRequestDescriptor<Example>(new Example()
        {
            String = "string 1",
        });

        var getResult = _elastic.Get<Example>(getDescriptor);

        var search = new SearchRequestDescriptor<Example>();
        search = search
            .From((request.Page - 1) * request.Size)
            .Size(request.Size);

        var query = _elastic.Search<Example>(search);
        var result = query.Documents;
        return result.Where(x => x.String == "string 1").ToList();
    }
}

public interface IExampleService
{
    Task<PagedResponse<ExampleDto>> GetPaged(GetPagedExampleRequest? request);
    Task<PagedResponse<ExampleDocument>> GetPaged(GetPagedExampleDocument? request);
    IReadOnlyCollection<Example> GetPaged(GetPagedExampleElastic? request);
}
