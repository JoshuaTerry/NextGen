using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DDI.Data.Helpers
{
    public static class EntityFrameworkHelpers
    {
        /// <summary>
        /// Specifies the related objects to include in query results.
        /// </summary>
        /// <typeparam name="T">The type of entity being queried.</typeparam>
        /// <typeparam name="TProperty">The type of navigation property being included.</typeparam>
        /// <param name="source">The source IQueryable on which to call Include</param>
        /// <param name="path">A lambda expression representing the path to include.</param>
        /// <returns>A new IQueryable<T> with the defined query path.</returns>
        public static IQueryable<T> IncludePath<T, TProperty>(IQueryable<T> source, System.Linq.Expressions.Expression<Func<T, TProperty>> path) where T : class
        {
            return source.Include(path);
        }
    }
}