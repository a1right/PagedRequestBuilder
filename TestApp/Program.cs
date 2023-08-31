using BenchmarkDotNet.Running;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Services;
using TestApp.Benchmarks;
using TestApp.Models.MongoDriver;

namespace PagedRequestTestApp;

public class Program
{
    public static void Main(string[] args)
    {
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
        //query providers
        builder.Services.AddDbContext<ExampleContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppContext")));
        builder.Services.AddScoped<IExampleService, ExampleService>();
        builder.Services.AddSingleton<ExampleMongoContext>();

        var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
                .CertificateFingerprint("bf08d117b8aa8e32dad4a8a715c10140bfa6740ef25c77d8d5bec962566618ea")
                .Authentication(new BasicAuthentication("elastic", "JCkMhZ-k-fx80op6+61l"))
                .DefaultIndex("example");

        var client = new ElasticsearchClient(settings);

        builder.Services.AddSingleton(client);

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
        var mongo = app.Services.GetService<ExampleMongoContext>();
        var elastic = app.Services.GetService<ElasticsearchClient>();
        context.Database.EnsureDeleted();
        var created = context.Database.EnsureCreated();
        if (created)
            Seed(context, mongo!, elastic!);

        PagedQueryBuilder.Initialize();
        app.Run();

    }
    private static void Seed(ExampleContext context, ExampleMongoContext mongoContext, ElasticsearchClient elastic)
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
                        String = $"double inner string {x}",
                        EnumArray = new[] { Enum.Parse<ExampleEnum>((x % 3).ToString()) },
                        EnumList = new List<ExampleEnum> { Enum.Parse<ExampleEnum>((x % 3).ToString()) }
                    }
                },
            }).ToList());
        }

        context.SaveChanges();

        daysShift = 0;
        var data = new List<ExampleDocument>();
        for (var i = 0; i < 5; i++)
        {
            data.AddRange((Enumerable.Range(1, 10).Select(x => new ExampleDocument()
            {
                Date = DateTime.UtcNow.AddDays(-daysShift++),
                Decimal = (decimal)(0.1 * x),
                Enum = (ExampleEnum)x,
                String = $"string {x}",
                Strings = new() { $"string {x}" },
                Guid = Guid.Parse($"CA0EA80A-322C-436D-8E23-C638A30CF8F{x % 10}"),
                Decimals = new List<decimal> { 0.1m * x },
                Ints = new[] { x }
            })));
        }

        mongoContext.Add(data);
        elastic.Indices.Delete("example");
        elastic.Indices.Create<Example>();
        var elasticData = context.Examples.ToList();
        elastic.IndexMany(elasticData, "example");
    }
}
