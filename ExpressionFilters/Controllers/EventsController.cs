using EventStuff.Models;
using EventStuff.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpressionFilters.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IEventService _eventService;

    public EventsController(ApplicationContext context, IEventService eventService)
    {
        _context = context;
        _eventService = eventService;
    }

    [HttpPost]
    public async Task<List<Event>> GetEvents([FromBody] EventFilter? filter)
    {
        var events = await _eventService.GetAll(filter);
        return events;
    }

    [HttpGet("test")]
    public async Task<List<Event>> Test()
    {
        var events = _context.Events.ToList();
        return events;
    }
}
