using System;
using System.Collections;

namespace PagedRequestBuilder.Common.MethodInfoProvider
{
    public class MethodInfoStrategyProvider : IMethodInfoStrategyProvider
    {
        private readonly StringMethodInfoStrategy _stringMethodInfoStrategy;
        private readonly EnumerableMethodInfoStrategy _enumerableMethodInfoStrategy;
        private readonly ArrayMethodInfoStrategy _arrayMethodInfoStrategy;

        public MethodInfoStrategyProvider(
            StringMethodInfoStrategy stringMethodInfoStrategy,
            EnumerableMethodInfoStrategy enumerableMethodInfoStrategy,
            ArrayMethodInfoStrategy arrayMethodInfoStrategy)
        {
            _stringMethodInfoStrategy = stringMethodInfoStrategy;
            _enumerableMethodInfoStrategy = enumerableMethodInfoStrategy;
            _arrayMethodInfoStrategy = arrayMethodInfoStrategy;
        }
        public IMethodInfoStrategy OfType(Type assignablePropertyType)
        {
            if (assignablePropertyType == typeof(string))
                return _stringMethodInfoStrategy;

            if (assignablePropertyType.IsArray)
                return _arrayMethodInfoStrategy;

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType))
                return _enumerableMethodInfoStrategy;

            throw new NotImplementedException(assignablePropertyType.FullName);
        }
    }

    public interface IMethodInfoStrategyProvider
    {
        IMethodInfoStrategy OfType(Type assignablePropertyType);
    }
}
