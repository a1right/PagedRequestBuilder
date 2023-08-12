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
        private readonly ApplicationContext _context;
        private readonly IFilterBuilder<Example> _filterBuilder;
        private readonly ISorterBuilder<Example> _sorterBuilder;

        public ExampleService(ApplicationContext context, IFilterBuilder<Example> filterBuilder, ISorterBuilder<Example> sorterBuilder)
        {
            _context = context;
            _filterBuilder = filterBuilder;
            _sorterBuilder = sorterBuilder;
        }

        public async Task<PagedResponse<ExampleDto>> GetPaged(GetPagedExampleRequest? request)
        {
            request ??= new();

            var filters = _filterBuilder.BuildFilters(request);
            var sorters = _sorterBuilder.BuildSorters(request);

            var query = _context.Examples
                .AsQueryable()
                .Where(filters)
                .OrderBy(sorters)
                .Paginate(request.Size, request.Page)
                .Select(x => x.MapTo<Example, ExampleDto>());

            var data = await query.ToListAsync();
            var total = await query.CountAsync();

            return data.ToPagedResponse(request.Page, request.Size, total);
        }
    }

    public interface IExampleService
    {
        Task<PagedResponse<ExampleDto>> GetPaged(GetPagedExampleRequest? request);
    }
}
