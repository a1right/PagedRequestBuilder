using EventStuff.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventStuff.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationContext _context;

        public EventService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Add(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Event>> GetAll(EventFilter? filter)
        {
            if (filter is null)
                return await _context.Events.ToListAsync();

            var query = ParseFilter(filter);

            return await query.ToListAsync();
        }

        private IQueryable<Event> ParseFilter(EventFilter filter)
        {
            var query = ParseProperties(_context.Events, filter.Properties);

            if (filter.Type is not null)
                query = query.Where(x => x.Type == filter.Type);

            if (filter.From is not null)
                query = query.Where(x => x.DateTime > filter.From);

            if (filter.To is not null)
                query = query.Where(x => x.DateTime < filter.To);

            return query;
        }

        private IQueryable<Event> ParseProperties(DbSet<Event> query, List<EventExtendedProperty>? properties)
        {
            if (properties is null)
                return query;

            var propertiesJson = JsonSerializer.Serialize(properties);

            //var sql = string.Format(EventQueries.PropertiesFilter, propertiesJson);
            Console.WriteLine(query.FromSqlRaw(EventQueries.PropertiesFilterStatic, propertiesJson).ToQueryString());

            return query.FromSqlRaw(EventQueries.PropertiesFilterStatic, propertiesJson);
        }
    }

    public static class EventQueries
    {
        public static string PropertiesFilter = "select * from \"Events\" e where \"Properties\" @> ''{0}''";
        public static string PropertiesFilterStatic = $"select * from \"Events\" e where \"Properties\" @> {{0}}::jsonb";
    }

    public interface IEventService
    {
        Task Add(Event @event);
        Task<List<Event>> GetAll(EventFilter? filter);
    }

}
