using System.Collections.Generic;

namespace DDI.Shared.Extensions
{
    public static class ListExtensions
    {
        public static void AddRangeNullSafe<T>(this List<T> self, IEnumerable<T> newCollection)
        {
            if (newCollection != null)
            {
                self.AddRange(newCollection);
            }
        }
    }
}
