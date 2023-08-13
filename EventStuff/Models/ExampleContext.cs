using Microsoft.EntityFrameworkCore;

namespace EventStuff.Models
{
    public class ExampleContext : DbContext
    {
        public DbSet<Example> Examples { get; set; }
        public ExampleContext(DbContextOptions options) : base(options) { }
    }
}
