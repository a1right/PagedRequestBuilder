using Microsoft.EntityFrameworkCore;

namespace EventStuff.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options) { }
    }
}
