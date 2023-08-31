using Microsoft.AspNetCore.Mvc;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Services;
using TestApp.Models.Elasticsearch;

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

    [HttpPost("mongo")]
    public async Task<IActionResult> GetPagedMongo([FromBody] GetPagedExampleDocument? request)
    {
        var response = await _exampleService.GetPaged(request);
        return Ok(response);
    }

    [HttpPost("elastic")]
    public async Task<IActionResult> GetPagedElastic([FromBody] GetPagedExampleElastic? request)
    {
        var response = _exampleService.GetPaged(request);
        return Ok(response);
    }
}
