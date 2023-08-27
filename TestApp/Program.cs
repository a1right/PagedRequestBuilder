using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Services;
using TestApp.Benchmarks;

namespace PagedRequestTestApp;

public class Program
{
    public static void Main(string[] args)
    {
        var test = new Test();
        test.Setup();
        test.BuildQuery_Caching_Enabled();
        test.CleanUp();
        return;
        var builder = WebApplication.CreateBuilder(args);
        var environment = builder.Environment;
        if (environment.IsEnvironment("benchmark"))
        {
            var summary = BenchmarkRunner.Run<Test>();
            return;
        }
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ExampleContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppContext")));
        builder.Services.AddScoped<IExampleService, ExampleService>();
        builder.Services.AddPagedQueryBuilder();

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
        using var context = scope.ServiceProvider.GetRequiredService<ExampleContext>();
        context.Database.EnsureDeleted();
        var created = context.Database.EnsureCreated();
        if (created)
            Seed(context);

        PagedQueryBuilder.Initialize();
        app.Run();

    }
    private static void Seed(ExampleContext context)
    {
        var daysShift = 0;
        for (var i = 0; i < 5; i++)
        {
            context.Examples.AddRange(Enumerable.Range(1, 10).Select(x => new Example()
            {
                Date = DateTime.UtcNow.AddDays(-daysShift++),
                Decimal = (decimal)(0.1 * x),
                Enum = (ExampleEnum)x,
                String = $"string {x}",
                Strings = new() { $"string {x}" },
                Guid = Guid.Parse($"CA0EA80A-322C-436D-8E23-C638A30CF8F{x % 10}"),
                Decimals = new List<decimal> { 0.1m * x },
                Ints = new[] { x },
                Inner = new()
                {
                    String = $"inner string {x}",
                    Nested = new()
                    {
                        String = $" double inner string {x}",
                        EnumArray = new[] { Enum.Parse<ExampleEnum>((x % 3).ToString()) },
                        EnumList = new List<ExampleEnum> { Enum.Parse<ExampleEnum>((x % 3).ToString()) }
                    }
                },
            }).ToList());
        }

        context.SaveChanges();
    }
}
