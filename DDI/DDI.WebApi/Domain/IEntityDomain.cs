 
using DDI.Shared;
using DDI.Shared.Models;

namespace DDI.WebApi.Domain
{
    public interface IEntityDomain<T> where T : class, IEntity
    {
        IRepository<T> Repository { get; }
    }
}
