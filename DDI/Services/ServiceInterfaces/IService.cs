using DDI.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.ServiceInterfaces
{
    public interface IService<T>
    {
        IDataResponse<List<T>> GetAll(IPageable search = null);
        IDataResponse<T> GetById(Guid id);
        IDataResponse Update(T entity);
        IDataResponse<T> Update(Guid id, JObject changes);
        IDataResponse<T> Add(T entity);
        IDataResponse Delete(T entity);
    }
}
