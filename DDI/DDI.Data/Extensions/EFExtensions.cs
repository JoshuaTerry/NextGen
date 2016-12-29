using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

public static class EFExtensions
{
    public static IQueryable<T> IncludePath<T,TProperty>(this IQueryable<T> self, System.Linq.Expressions.Expression<Func<T,TProperty>> predicate) where T : class
    {
        return self.Include(predicate);
    }
}
