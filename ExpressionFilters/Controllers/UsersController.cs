using ExpressionFilters.Models;
using GenericFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressionFilters.Controllers;
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IQueryBuilder _queryBuilder;
    private readonly IFilterBuilder _filterBuilder;

    public UsersController(ApplicationContext context, IQueryBuilder queryBuilder, IFilterBuilder filterBuilder)
    {
        _context = context;
        _queryBuilder = queryBuilder;
        _filterBuilder = filterBuilder;
    }

    [HttpGet("u")]
    public async Task<IActionResult> GetUsers(string? request)
    {
        var filter = _filterBuilder.BuildFilter(request);
        var query = _queryBuilder.BuildQuery(_context.Users.AsQueryable(), filter);
        var users = await query.ToListAsync();
        return Ok(users);
    }

    [HttpGet("o")]
    public async Task<IActionResult> GetOrganizations(string? filter)
    {
        var response = await _context.Organizations
            .BuildQuery(filter)
            .Include(x => x.Users)
            .ToListAsync();

        return Ok(response);
    }
}
