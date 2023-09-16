using Microsoft.AspNetCore.Mvc;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Services;

namespace PagedRequestTestApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamplesController : ControllerBase
{
    private readonly IExampleService _exampleService;

    public ExamplesController(IExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    [HttpPost("relational")]
    public async Task<IActionResult> GetPagedRelational([FromBody] GetPagedExampleRequest? request)
    {
        var response = await _exampleService.GetPaged(request);
        return Ok(response);
    }
}
