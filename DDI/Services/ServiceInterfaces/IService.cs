using DDI.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DDI.Services.ServiceInterfaces
{
    public interface IService<T>
    {
        IDataResponse<List<T>> GetAll();
        IDataResponse<T> GetById(Guid id);
        IDataResponse Update(T entity);
        IDataResponse<T> Update(Guid id, JObject changes);
        IDataResponse Add(T entity);
        IDataResponse Delete(T entity);
    }
}
