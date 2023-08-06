using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericFilters
{
    public class QueryBuilder : IQueryBuilder
    {

        public IQueryable<T> BuildQuery<T>(IQueryable<T> query, Filter requestFilters)
        {
            return query.Where(BuildQueryExpression<T>(requestFilters));
        }

        private Expression<Func<T, bool>> BuildQueryExpression<T>(Filter requestFilters)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var body = BuildQueryExpressionInternal<T>(requestFilters, parameter);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private Expression? BuildQueryExpressionInternal<T>(Filter requestFilters, ParameterExpression parameter)
        {
            if (requestFilters is SimpleFilter simpleFilter)
                return BuildQuerySimpleFilterEpression<T>(simpleFilter, parameter);

            else if (requestFilters is ComplexFilter complexFilter)
            {
                Expression? body = null;

                foreach (var filter in complexFilter.Filters!)
                {
                    var operation = BuildQueryExpressionInternal<T>(filter, parameter);
                    body = body == null ? operation : Expression.AndAlso(body, operation);
                }

                return body ?? Expression.Constant(true);
            }

            throw new NotImplementedException();
        }

        private Expression BuildQuerySimpleFilterEpression<T>(SimpleFilter filter, ParameterExpression parameter)
        {
            var left = Expression.PropertyOrField(parameter, filter.Property);

            return filter.Operator switch
            {
                "$gt" => Expression.GreaterThan(left, Expression.Constant(Parse(filter.Value, typeof(T).GetProperty(filter.Property)))),
                "$eq" => Expression.Equal(left, Expression.Constant(filter.Value)),
                "$count" => Expression.Equal(Expression.Constant(Expression.PropertyOrField(left, "Count")), Expression.Constant(int.Parse(filter.Value))),
                _ => throw new NotImplementedException(),
            };
        }

        private object? Parse(string? value, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(int))
                return int.Parse(value);

            if (propertyInfo.PropertyType == typeof(double))
                return double.Parse(value);

            if (propertyInfo.PropertyType == typeof(DateTime))
                return DateTime.SpecifyKind(DateTime.Parse(value), DateTimeKind.Utc);

            throw new NotImplementedException();
        }
    }
}
