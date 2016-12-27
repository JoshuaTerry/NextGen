using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
    public static class DbEntityEntryExtensions
    {
        #region Public Methods

        //public static DbEntityEntry<T> Include<T>(this DbEntityEntry<T> self, params Expression<Func<T, object>>[] includes) 
        //    where T : class
        //{
        //    Type entityType = typeof(T);
        //    T entity = EntitySet.Find(id);
        //    DbEntityEntry<T> dbEntry = _context.Entry(entity);

        //    foreach (Expression<Func<T, object>> include in includes)
        //    {
        //        try
        //        {
        //            string property = NameFor(include, true);
        //            PropertyInfo prop = entityType.GetProperty(property);

        //            if (prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
        //            {
        //                dbEntry.Collection(property).Load();
        //            }
        //            else if (!prop.PropertyType.IsValueType)
        //            {
        //                dbEntry.Reference(property).Load();
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            Console.WriteLine(exception);
        //        }
        //    }
        //}

        #endregion Public Methods
    }
}
