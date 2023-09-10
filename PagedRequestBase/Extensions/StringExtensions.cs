namespace PagedRequestBuilder.Extensions;

internal static class StringExtensions
{
    public static string Format(this string template, params object[] args) => string.Format(template, args);
}
