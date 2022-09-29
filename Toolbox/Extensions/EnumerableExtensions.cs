using System.Linq;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static T MaxBy<T>(this IEnumerable<T> enumerable, Func<T, DateTimeOffset?> selector)
        {
            if (enumerable?.Any() != true)
            {
                return default;
            }

            var maxValue = enumerable.Max(r => selector(r));
            return enumerable.First(t => maxValue == selector(t));
        }

        public static T MaxBy<T>(this IEnumerable<T> enumerable, Func<T, double> selector)
        {
            if (enumerable?.Any() != true)
            {
                return default;
            }

            var maxValue = enumerable.Max(r => selector(r));
            return enumerable.First(t => maxValue == selector(t));
        }
    }
}
