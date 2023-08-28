using PagedRequestBuilder.Models;
using System.Collections.Generic;

namespace PagedRequestBuilder.Extensions;

public static class MappingExtensions
{
    public static PagedResponse<T> ToPagedResponse<T>(this List<T> data, int? page, int? size, int? total) =>
        new(data, page, size, total);
}
