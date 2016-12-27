using DDI.Data;
using DDI.Data.Models;

namespace DDI.WebApi.Domain
{
    public interface IEntityDomain<T> where T : class, IEntity
    {
        IRepository<T> Repository { get; }
    }
}
