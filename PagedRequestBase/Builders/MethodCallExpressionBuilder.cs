using PagedRequestBuilder.Common.MethodInfoProvider;
using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Builders
{
    public class MethodCallExpressionBuilder : IMethodCallExpressionBuilder
    {
        private readonly IMethodInfoProvider _methodInfoProvider;

        public MethodCallExpressionBuilder(IMethodInfoProvider methodInfoProvider)
        {
            _methodInfoProvider = methodInfoProvider;
        }

        public Expression Build(string name, MemberExpression left, ConstantExpression right, Type assignablePropertyType)
        {
            var method = _methodInfoProvider.GetMethodInfo(name, assignablePropertyType);

            if (method.IsStatic)
                return Expression.Call(method, left, right);

            return Expression.Call(left, method, right);
        }
    }

    public interface IMethodCallExpressionBuilder
    {
        Expression Build(string name, MemberExpression left, ConstantExpression right, Type assignablePropertyType);
    }
}
