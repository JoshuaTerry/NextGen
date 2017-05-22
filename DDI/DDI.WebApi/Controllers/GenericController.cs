using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using System;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DDI.WebApi.Controllers
{
    public class GenericController<T> : ControllerBase<T> where T : class, IEntity
    {
        public GenericController() :this(Factory.CreateService<ServiceBase<T>>())
        {
        }

        public GenericController(IService<T> serviceBase) : base(serviceBase, new DynamicTransmogrifier(), new Pagination())
        {
        }
         
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