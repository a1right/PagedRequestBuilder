using Microsoft.AspNetCore.Mvc;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Services;

namespace PagedRequestTestApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamplesController : ControllerBase
{
    private readonly ExampleContext _context;
    private readonly IExampleService _exampleService;

    public ExamplesController(ExampleContext context, IExampleService exampleService)
    {
        _context = context;
        _exampleService = exampleService;
    }

    [HttpPost]
    public async Task<IActionResult> GetEvents([FromBody] GetPagedExampleRequest? request)
    {
        var response = await _exampleService.GetPaged(request);
        return Ok(response);
    }
}
