using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using DDIX.Module.Helpers;
using LinqKit;

namespace DDI.Business.Helpers
{
    public class CriteriaQuery<TEntity> : LinqQuery<TEntity>
    {
        public CriteriaQuery(IQueryable<TEntity> query) 
            : base(query)
        {
        }

        public CriteriaQuery<TEntity> And(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                return this;
            }

            PredicateAnd(expression);

            return this;
        }
    }
}