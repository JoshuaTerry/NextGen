using System;
using System.Collections.Generic;
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

        #endregion Public Properties

        #region Public Methods

        void Delete(T entity);

        T GetById(object id);

        T Find(params object[] keyValues); 

		T Insert(T entity);

		T Update(T entity);

        int UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues);

        #endregion Public Methods
    }
}
