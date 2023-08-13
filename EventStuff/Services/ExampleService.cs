using EventStuff.Builders;
using EventStuff.Extensions;
using EventStuff.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EventStuff.Services
{
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
                .Select(x => x.Map<Example, ExampleDto>());

            var data = await pagedQuery.ToListAsync();
            var total = await query.CountAsync();

            return data.ToPagedResponse(request.Page, request.Size, total);
        }
    }

    public interface IExampleService
    {
        Task<PagedResponse<ExampleDto>> GetPaged(GetPagedExampleRequest? request);
    }
}
