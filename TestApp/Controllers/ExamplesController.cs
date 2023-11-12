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

    [HttpGet("relational")]
    public async Task<IActionResult> GetPagedRelational([FromQuery] GetPagedExampleRequest? request)
    {
        try
        {
            var response = await _exampleService.GetPaged(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
