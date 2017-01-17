using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
