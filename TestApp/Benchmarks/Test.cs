using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Services;
using System.Runtime.CompilerServices;

namespace TestApp.Benchmarks;

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
                new () { Property = "decimals", Value = 0.2m, Operation = "contains"}
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
        var query = _builder.BuildQuery(_query, _request);
    }

}
