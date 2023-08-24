using PagedRequestBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PagedRequestBuilder.Extensions
{
    public static class MappingExtensions
    {
        public static PagedResponse<T> ToPagedResponse<T>(this List<T> data, int? page, int? size, int? total)
        {
            return new PagedResponse<T>(data, page, size, total);
        }

        public static PagedResponse<TProjection> ToResponse<T, TProjection>(this IEnumerable<T> data, int page, int size, int total, Func<T, TProjection> projection)
        {
            return new PagedResponse<TProjection>(data.Select(projection).ToList(), page, size, total);
        }

        public static TProjection MapTo<T, TProjection>(this T src) where T : class
        {
            var serialized = JsonSerializer.Serialize(src);
            return JsonSerializer.Deserialize<TProjection>(serialized)!;
        }

        public static TProjection Map<T, TProjection>(this T src) where T : class
        {
            var serialized = JsonSerializer.Serialize(src);
            return JsonSerializer.Deserialize<TProjection>(serialized)!;
        }
    }
}
