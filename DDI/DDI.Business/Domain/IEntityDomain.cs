using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Data.Models;

namespace DDI.Business.Domain
{
    public interface IEntityDomain<T> where T : class, IEntity
    {
        IRepository<T> Repository { get; }
    }
}
