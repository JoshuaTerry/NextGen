using System.Collections.Generic;
using DDI.Shared;
using DDI.WebApi.Services.Search;

namespace DDI.WebApi.Services
{
    public interface IGenericService<T>
    {
        IDataResponse<dynamic> GetAll(IPageable searchTerms = null);
        IDataResponse Delete(T entity);
        IDataResponse Add(T entity);
        IDataResponse Update(T entity);

    }
}