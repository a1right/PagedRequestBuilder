using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ExpressionFilters.Models;

public class User
{
    public int Id { get; set; }
    public int Age { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public DateTime Registered { get; set; }
}
public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; } = new();
}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    public ApplicationContext(DbContextOptions options) : base(options) { }
}