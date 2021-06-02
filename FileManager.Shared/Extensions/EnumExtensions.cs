using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManager.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> FromFlags<T>(this T value) where T : struct, Enum
            => Enum.GetValues(typeof(T)).Cast<T>().Distinct().Where(x => value.HasFlag(x));

        public static IEnumerable<T> FromFlags<T>(this T? value) where T : struct, Enum
            => value.HasValue ? value.Value.FromFlags() : null;

        public static T CombineFlags<T>(this IEnumerable<T> values) where T : struct, Enum
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            var enumType = typeof(T).GetEnumUnderlyingType();
            if (enumType == typeof(int))
                return (T)Enum.ToObject(typeof(T), values.Cast<int>().Aggregate((a, b) => a | b));
            // add support for uint/long/etc here
            throw new NotSupportedException("Enum type not supported");
        }
    }
}
