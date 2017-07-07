using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DDI.Shared
{
    public interface IService<T> : IService
    {
        IDataResponse<List<ICanTransmogrify>> GetAll();
        IDataResponse<List<ICanTransmogrify>> GetAll(string fields, IPageable search = null);
        IDataResponse<T> GetById(Guid id);
        IDataResponse Update(T entity);
        IDataResponse<T> Update(Guid id, JObject changes);
        IDataResponse<T> Update(T entity, JObject changes);
        IDataResponse<T> Add(T entity);
        IDataResponse Delete(T entity);
        IDataResponse<T> GetWhereExpression(Expression<Func<T, bool>> expression);
        IDataResponse<List<ICanTransmogrify>> GetAllWhereExpression(Expression<Func<T, bool>> expression, IPageable search = null, string fields = null);
        Expression<Func<T, object>>[] IncludesForSingle { set; }
        Expression<Func<T, object>>[] IncludesForList { set; }

    }

    public interface IService
    {
    }
}
