using System;
using System.Runtime.CompilerServices;

namespace PagedRequestBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PagedRequestKeyAttribute : Attribute
    {
        public string RequestKey { get; }
        public string? TargetProperty { get; }
        public PagedRequestKeyAttribute(string key, [CallerMemberName] string? property = null)
        {
            RequestKey = key;
            TargetProperty = property;
        }
    }
}
