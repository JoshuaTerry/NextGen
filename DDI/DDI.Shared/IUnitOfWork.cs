using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IUnitOfWork
    {
        #region Public Methods

        IRepository<T> GetRepository<T>() where T : class;
        void SetRepository<T>(IRepository<T> repository) where T : class;

        int SaveChanges();

        #endregion Public Methods
    }
}
