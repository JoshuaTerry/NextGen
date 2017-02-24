using DDI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IReadOnlyRepository<T> where T: class, IReadOnlyEntity
    {
        #region Public Properties

        IQueryable<T> Entities { get; }
         
        #endregion Public Properties

        #region Public Methods
         
        T GetById(Guid id);

        T GetById(Guid id, params Expression<Func<T, object>>[] includes);

        T Find(params object[] keyValues);
        
        void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class;

        void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class;

        ICollection<TElement> GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class;

        TElement GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class;

        ICollection<T> GetLocal();
         
        IQueryable<T> GetEntities(params Expression<Func<T, object>>[] includes);

        #endregion Public Methods

    }
}
