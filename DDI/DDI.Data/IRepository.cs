using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data
{
    /// <summary>
    /// Defines the required functionality for a class to act as a "Repository" by accessing and
    /// returning entities from a data store.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        #region Public Properties

        IQueryable<T> Entities { get; }

        SQLUtilities Utilities { get; }

        #endregion Public Properties

        #region Public Methods

        void Delete(T entity);

        T GetById(object id);

        T Find(params object[] keyValues);

        T Create();

        T Insert(T entity);

		T Update(T entity);

        int UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null);

        int UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null);

        List<string> GetModifiedProperties(T entity);

        void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class;

        void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class;

        ICollection<TElement> GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class;

        TElement GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class;

        ICollection<T> GetLocal();

        void Attach(T entity);

        #endregion Public Methods
    }
}
