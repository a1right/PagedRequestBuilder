using Moq;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Common.MethodInfoProvider;
using PagedRequestBuilder.Common.ValueParser;
using PagedRequestBuilder.Models.Filter;
using PagedRequestBuilder.Tests.Infrastructure.Builders.FilterBuilder;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Tests.Builders;
public class FilterBuilderTests
{
    private readonly FilterBuilder<FilterBuilderTestClass> _builder;
    public FilterBuilderTests()
    {
        var filterCacheMock = new Mock<IQueryFilterCache<FilterBuilderTestClass>>();
        filterCacheMock
            .Setup(x => x.Get(new()))
            .Returns<IQueryFilter<FilterBuilderTestClass>>(null);

        filterCacheMock
            .Setup(x => x.Set(null, null));

        var methodInfoCacheMock = new Mock<IMethodInfoCache>();
        methodInfoCacheMock
            .Setup(x => x.Get(string.Empty, typeof(object)))
            .Returns<MethodInfo>(null);

        methodInfoCacheMock
            .Setup(x => x.Set(null, null, null));

        _builder = new FilterBuilder<FilterBuilderTestClass>(
            new PagedRequestValueParser(new ValueParseStrategyProvider(new(), new(), new())),
            new RequestPropertyMapper(),
            filterCacheMock.Object,
            new MethodCallExpressionBuilder(new MethodInfoProvider(methodInfoCacheMock.Object, new MethodInfoStrategyProvider(new(), new(), new())))
            );

        PagedQueryBuilder.Initialize();
    }

    [Theory]
    [InlineData(FilterBuilderTestClass.IdPagedKey, 1, Constants.Strings.RequestOperations.Equal)]
    [InlineData(FilterBuilderTestClass.IdPagedKey, 1, Constants.Strings.RequestOperations.NotEqual)]
    [InlineData(FilterBuilderTestClass.IdPagedKey, 1, Constants.Strings.RequestOperations.GreaterThen)]
    [InlineData(FilterBuilderTestClass.IdPagedKey, 1, Constants.Strings.RequestOperations.GreaterThenOrEquals)]
    [InlineData(FilterBuilderTestClass.IdPagedKey, 1, Constants.Strings.RequestOperations.LessThen)]
    [InlineData(FilterBuilderTestClass.IdPagedKey, 1, Constants.Strings.RequestOperations.LessThenOrEqual)]
    [InlineData(FilterBuilderTestClass.IdPagedKey, new[] { 1, 2, 3 }, Constants.Strings.RequestOperations.In)]
    //
    [InlineData(FilterBuilderTestClass.StringPagedKey, "test", Constants.Strings.RequestOperations.Equal)]
    [InlineData(FilterBuilderTestClass.StringPagedKey, "test", Constants.Strings.RequestOperations.NotEqual)]
    [InlineData(FilterBuilderTestClass.StringPagedKey, new[] { "test", "test", "test" }, Constants.Strings.RequestOperations.In)]
    [InlineData(FilterBuilderTestClass.StringPagedKey, "test", Constants.Strings.RequestOperations.Contains)]
    //
    [InlineData(FilterBuilderTestClass.GuidPagedKey, "ca0ea80a-322c-436d-8e23-c638a30cf8f0", Constants.Strings.RequestOperations.Equal)]
    [InlineData(FilterBuilderTestClass.GuidPagedKey, "ca0ea80a-322c-436d-8e23-c638a30cf8f0", Constants.Strings.RequestOperations.NotEqual)]
    [InlineData(FilterBuilderTestClass.GuidPagedKey, new[] { "ca0ea80a-322c-436d-8e23-c638a30cf8f0", "ca0ea80a-322c-436d-8e23-c638a30cf8f1", "ca0ea80a-322c-436d-8e23-c638a30cf8f2" }, Constants.Strings.RequestOperations.In)]
    [InlineData(FilterBuilderTestClass.GuidPagedKey, "ca0ea80a-322c-436d-8e23-c638a30cf8f0", Constants.Strings.RequestOperations.Contains)]

    public void BuildFilters_ReturnsValidFilters(string property, object value, string operation)
    {
        //arrange
        var request = new FilterBuilderTestRequest()
        {
            Filters = new()
            {
                new()
                {
                    Property = property,
                    Value = JsonNode.Parse(JsonSerializer.Serialize(value)),
                    Operation = operation
                }
            }
        };

        var actual = _builder.BuildFilters(request).First();
        var expected = GetExpectedPredicate(property, operation, value);

        var equals = actual.ToString() == expected.ToString();
        Assert.True(equals);
    }

    private Expression<Func<FilterBuilderTestClass, bool>> GetExpectedPredicate(string property, string operation, object value)
    {
        var type = value.GetType();

        if (type == typeof(int))
            return GetIntExpectedPredicate(property, operation, (int)value);

        if (type == typeof(string) && !Guid.TryParse((string)value, out _))
            return GetStringExpectedPredicate(property, operation, (string)value);

        if (type == typeof(string) && Guid.TryParse((string)value, out var guid))
            return GetGuidExpectedPredicate(property, operation, guid);

        throw new NotImplementedException();
    }

    private Expression<Func<FilterBuilderTestClass, bool>> GetIntExpectedPredicate(string property, string operation, int value) => (property, operation) switch
    {
        (FilterBuilderTestClass.IdPagedKey, Constants.Strings.RequestOperations.Equal) => x => x.Id == value,
        (FilterBuilderTestClass.IdPagedKey, Constants.Strings.RequestOperations.NotEqual) => x => x.Id != value,
        (FilterBuilderTestClass.IdPagedKey, Constants.Strings.RequestOperations.GreaterThen) => x => x.Id > value,
        (FilterBuilderTestClass.IdPagedKey, Constants.Strings.RequestOperations.GreaterThenOrEquals) => x => x.Id >= value,
        (FilterBuilderTestClass.IdPagedKey, Constants.Strings.RequestOperations.LessThen) => x => x.Id < value,
        (FilterBuilderTestClass.IdPagedKey, Constants.Strings.RequestOperations.LessThenOrEqual) => x => x.Id <= value,
    };

    private Expression<Func<FilterBuilderTestClass, bool>> GetIntExpectedPredicate(string property, string operation, int[] value) => (property, operation) switch
    {
        (FilterBuilderTestClass.IdPagedKey, Constants.Strings.RequestOperations.In) => x => value.Contains(x.Id),
    };

    private Expression<Func<FilterBuilderTestClass, bool>> GetStringExpectedPredicate(string property, string operation, string value) => (property, operation) switch
    {
        (FilterBuilderTestClass.StringPagedKey, Constants.Strings.RequestOperations.Equal) => x => x.String == value,
        (FilterBuilderTestClass.StringPagedKey, Constants.Strings.RequestOperations.NotEqual) => x => x.String != value,
        (FilterBuilderTestClass.StringPagedKey, Constants.Strings.RequestOperations.Contains) => x => x.String.Contains(value),
    };

    private Expression<Func<FilterBuilderTestClass, bool>> GetStringExpectedPredicate(string property, string operation, string[] value) => (property, operation) switch
    {
        (FilterBuilderTestClass.StringPagedKey, Constants.Strings.RequestOperations.In) => x => value.Contains(x.String),
    };

    private Expression<Func<FilterBuilderTestClass, bool>> GetGuidExpectedPredicate(string property, string operation, Guid value) => (property, operation) switch
    {
        (FilterBuilderTestClass.GuidPagedKey, Constants.Strings.RequestOperations.Equal) => x => x.Guid == value,
        (FilterBuilderTestClass.GuidPagedKey, Constants.Strings.RequestOperations.NotEqual) => x => x.Guid != value,
    };

    private Expression<Func<FilterBuilderTestClass, bool>> GetGuidExpectedPredicate(string property, string operation, Guid[] value) => (property, operation) switch
    {
        (FilterBuilderTestClass.GuidPagedKey, Constants.Strings.RequestOperations.In) => x => value.Contains(x.Guid),
    };
}
