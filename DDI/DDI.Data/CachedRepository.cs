using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DDI.Data
{
    public class CachedRepository<T> : Repository<T> where T: class
    {
        public CachedRepository() :
            base(new DomainContext())
        {
        }
         

        public CachedRepository(DbContext context) : base(context)
        {
            var type = typeof(T);
            string cachekey = $"{type.Name}_Key";
        }
        protected override IDbSet<T> EntitySet
        {
            get
            {
                //if (HttpContext.Current.Cache[]
                    
                
                    return base.EntitySet;
            }
        }
    }
}
