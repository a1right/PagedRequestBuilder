﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PagedRequestBuilder.Builders;

namespace PagedRequestBuilder.Models;

public class ExampleContext : DbContext
{
    public DbSet<Example> Examples { get; set; }
    public ExampleContext(DbContextOptions options) : base(options) { }
}

public class ExampleMongoContext
{
    private readonly IMongoCollection<ExampleDocument> _examples;
    private readonly IPagedQueryBuilder<ExampleDocument> _pagedQueryBuilder;

    public ExampleMongoContext()
    {
        var mongoClient = new MongoClient("mongodb://user:password@localhost:27017/");
        var mongoDatabase = mongoClient.GetDatabase("ExpressionFilters");
        mongoDatabase.DropCollection("ExampleDocument");
        _examples = mongoDatabase.GetCollection<ExampleDocument>("ExampleDocument");
    }

    public IMongoQueryable<ExampleDocument> Examples => _examples.AsQueryable();

    public void Add(IEnumerable<ExampleDocument> data) => _examples.InsertMany(data);
}
