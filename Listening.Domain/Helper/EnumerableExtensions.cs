namespace Listening.Domain.Helper
{
    public static class EnumerableExtensions
    {
        public static bool SequenceIgnoredEqual<T>(this IEnumerable<T> items1, IEnumerable<T> items2)
        {
            if (items1 == items2)
            {
                return true;
            }
            else if (items1 == null || items2 == null)
            {
                return false;
            }
            else
            {
                return items1.OrderBy(e => e).SequenceEqual(items2.OrderBy(e => e));
            }
        }
    }
}
