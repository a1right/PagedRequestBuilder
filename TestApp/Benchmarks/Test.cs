using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Services;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TestApp.Benchmarks;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

public class Test
{
    private IPagedQueryBuilder<Example> _builder;
    private IServiceScope _scope;
    private IQueryable<Example> _query;
    private GetPagedExampleRequest _request;

    [GlobalSetup]
    public void Setup()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddDbContext<ExampleContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppContext")));
        builder.Services.AddScoped<IExampleService, ExampleService>();
        builder.Services.AddPagedQueryBuilder();

        var app = builder.Build();
        PagedQueryBuilder.Initialize();
        _scope = app.Services.CreateScope();
        _builder = _scope.ServiceProvider.GetService<IPagedQueryBuilder<Example>>();
        _query = _scope.ServiceProvider.GetService<ExampleContext>().Examples.AsQueryable();

        _request = new GetPagedExampleRequest()
        {
            Filters = new()
            {
                new () { Property = "decimals", Value = JsonNode.Parse( JsonSerializer.Serialize(new []{ 0.5m,0.6m,0.4m,0.1m})), Operation = "in"},
                new () { Property = "string", Value = JsonNode.Parse(JsonSerializer.Serialize("ing 2")), Operation = "contains"},
                new () { Property = "decimal", Value = JsonNode.Parse(JsonSerializer.Serialize(0.2m)), Operation = ">"},
                new () { Property = "decimal", Value = JsonNode.Parse(JsonSerializer.Serialize(0.8m)), Operation = "<"},
                new () { Property = "guid", Value = JsonNode.Parse(JsonSerializer.Serialize("CA0EA80A-322C-436D-8E23-C638A30CF8F6")), Operation = "="},
                new () { Property = "inner", Value = JsonNode.Parse(JsonSerializer.Serialize(" double inner string 5")), Operation = "contains", Nested = new []{ "doubleinner", "string"} },
            },
            Page = 1,
            Size = 10,
        };
    }

    [GlobalCleanup]
    public void CleanUp() => _scope.Dispose();

    [Benchmark]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void BuildQuery_Caching_Enabled()
    {
        var query = _query;

        for (var i = 1; i < 100; i++)
        {
            query = _builder.BuildQuery(_query, _request);
        }

        Console.WriteLine(query.ToQueryString());
    }
}