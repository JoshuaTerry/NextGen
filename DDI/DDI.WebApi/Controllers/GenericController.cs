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
         
        public IHttpActionResult GetAll(string routeName, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null, UrlHelper urlHelper = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            return GetAll(routeName, search, fields, urlHelper ?? GetUrlHelper());
        }

        public IHttpActionResult GetAll(string routeName, IPageable search, string fields = null, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                return FinalizeResponse(Service.GetAll(fields, search), routeName, search, ConvertFieldList(fields, FieldsForList), urlHelper);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }
    }
}