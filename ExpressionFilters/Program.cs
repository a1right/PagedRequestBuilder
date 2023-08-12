using EventStuff.Builders;
using EventStuff.Models;
using EventStuff.Services;
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
        builder.Services.AddScoped<IExampleService, ExampleService>();
        builder.Services.AddScoped(typeof(IFilterBuilder<>), typeof(FilterBuilder<>));
        builder.Services.AddScoped(typeof(ISorterBuilder<>), typeof(SorterBuilder<>));
        builder.Services.AddScoped<IPagedRequestValueParser, PagedRequestValueParser>();

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
        context.Database.EnsureDeleted();
        var created = context.Database.EnsureCreated();
        if (created)
            Seed(context);

        app.Run();

    }

    private static void Seed(ApplicationContext context)
    {
        var daysShift = 0;
        for (var i = 0; i < 5; i++)
        {
            context.Examples.AddRange(Enumerable.Range(1, 10).Select(x => new Example()
            {
                Date = DateTime.UtcNow.AddDays(-daysShift++),
                Decimal = (decimal)(0.1 * x),
                Enum = (ExampleEnum)x,
                String = $"String {x}",
                Guid = Guid.Parse($"CA0EA80A-322C-436D-8E23-C638A30CF8F{x % 10}"),
            }).ToList());
        }
        context.SaveChanges();
    }
}
