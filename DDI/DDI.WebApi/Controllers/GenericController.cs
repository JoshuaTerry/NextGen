using System;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class GenericController<T> : ControllerBase<T> where T : class, IEntity
    {
        public GenericController(IService<T> serviceBase) : base(serviceBase) { }
         
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            return GetAll(search, fields);
        }

        public IHttpActionResult GetAll(IPageable search, string fields = null)
        {
            try
            { 
                fields = ConvertFieldList(fields, FieldsForList);
                return FinalizeResponse(Service.GetAll(fields, search), search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }
    }
}