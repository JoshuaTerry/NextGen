using System.Collections.Generic;
using DDI.Shared;


namespace DDI.WebApi.Services
{
    public interface IGenericService<T>
    {
        IDataResponse<List<T>> GetAll(IPageable searchTerms = null);
        IDataResponse Delete(T entity);
        IDataResponse Add(T entity);
        IDataResponse Update(T entity);

    }
}