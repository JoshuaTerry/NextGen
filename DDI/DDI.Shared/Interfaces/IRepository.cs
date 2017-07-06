using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DDI.Shared
{
    /// <summary>
    /// Defines the required functionality for a class to act as a "Repository" by accessing and
    /// returning entities from a data store.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        #region Public Properties

        IQueryable<T> Entities { get; }

        ISQLUtilities Utilities { get; }

        #endregion Public Properties

        #region Public Methods

        EntityState GetEntityState(T entity);

        void Delete(T entity);

        T GetById(Guid id);

        T GetById(Guid id, params Expression<Func<T, object>>[] includes);

        T Find(params object[] keyValues);

        T Create();

        T Insert(T entity);

        T Update(T entity);

        void UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null);

        void UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null);

        List<string> GetModifiedProperties(T entity);

        void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class;

        void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class;

        ICollection<TElement> GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class;

        TElement GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class;

        ICollection<T> GetLocal();

        T Attach(T entity);

        IQueryable<T> GetEntities(params Expression<Func<T, object>>[] includes);

        #endregion Public Methods
    }

    //
    // Summary:
    //     Describes the state of an entity.
    [Flags]
    public enum EntityState
    {
        //
        // Summary:
        //     The entity is not being tracked by the context. An entity is in this state immediately
        //     after it has been created with the new operator or with one of the System.Data.Entity.DbSet
        //     Create methods.
        Detached = 1,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database, and its
        //     property values have not changed from the values in the database.
        Unchanged = 2,
        //
        // Summary:
        //     The entity is being tracked by the context but does not yet exist in the database.
        Added = 4,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database, but has
        //     been marked for deletion from the database the next time SaveChanges is called.
        Deleted = 8,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database, and some
        //     or all of its property values have been modified.
        Modified = 16
    }

    // Defines non-generic versions of properties and methods.  (These are added only if needed.)
    public interface IRepository
    {
        IQueryable Entities { get; }
    }

}
