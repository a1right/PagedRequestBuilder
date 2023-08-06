using ExpressionFilters.Models;
using GenericFilters;
using Microsoft.EntityFrameworkCore;

namespace ExpressionFilters;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppContext")));
        builder.Services.AddExpressionFilter();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        var created = context.Database.EnsureCreated();
        if (created)
            Seed(context);

        app.Run();

    }

    private static void Seed(ApplicationContext context)
    {
        var users = Enumerable.Range(1, 10).Select(x => new User()
        {
            Age = x * 10,
            Name = $"User {x}",
            Registered = DateTime.UtcNow.AddDays(-x)
        });

        context.Users.AddRange(users);
        context.SaveChanges();

        var organizations = new List<Organization>()
        {
            new()
            {
                Name = "Organization 1",
                Users = users.Where(x => x.Age < 60).ToList(),
            },
            new()
            {
                Name = "Organization 2",
                Users = users.Where(x => x.Age > 60).ToList(),
            },
        };

        context.Organizations.AddRange(organizations);

        context.SaveChanges();
    }
}
