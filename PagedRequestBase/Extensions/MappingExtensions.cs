using EventStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EventStuff.Extensions
{
    public static class MappingExtensions
    {
        public static PagedResponse<T> ToPagedResponse<T>(this List<T> data, int? page, int? size, int? total)
        {
            return new PagedResponse<T>(data, page, size, total);
        }
    }
}
