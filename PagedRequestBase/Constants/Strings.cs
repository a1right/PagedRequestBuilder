namespace PagedRequestBuilder.Constants;
internal static class Strings
{
    public static class RequestOperations
    {
        public const string Equal = "=";
        public const string NotEqual = "!=";
        public const string GreaterThen = ">";
        public const string GreaterThenOrEquals = ">=";
        public const string LessThen = "<";
        public const string LessThenOrEqual = "<=";
        public const string Contains = "contains";
        public const string In = "in";
    }

    public static class MethodInfoNames
    {
        public const string Contains = "Contains";
    }

    public static class Errors
    {
        public static class Static
        {
        }

        public static class Templates
        {
            public const string TypeNotContainsPagedRequestKey = "Type '{0}' do not contains paged request key '{1}'";
        }
    }
}
