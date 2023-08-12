using Microsoft.EntityFrameworkCore;

namespace EventStuff.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Example> Examples { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options) { }
    }
}
