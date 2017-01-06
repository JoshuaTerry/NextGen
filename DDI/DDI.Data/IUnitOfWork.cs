using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data
{
    public interface IUnitOfWork : IDisposable
    {
        #region Public Methods

        IRepository<T> GetRepository<T>() where T : class;

        void SetRepository<T>(IRepository<T> repository) where T : class;

        int SaveChanges();

        IQueryable<T> Where<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class;

        IQueryable<T> GetEntities<T>() where T : class;

        T FirstOrDefault<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class;

        void LoadReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class where T : class;

        void LoadReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class where T : class;

        ICollection<TElement> GetReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class where T : class;

        TElement GetReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class where T : class;
        
        ICollection<T> GetLocal<T>() where T : class;

        void Attach<T>(T entity) where T : class;

        T Create<T>() where T : class;

        void Insert<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        void AddBusinessLogic(object blObj);

        T GetBusinessLogic<T>() where T : class;

        #endregion Public Methods
    }
}
