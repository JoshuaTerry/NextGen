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

namespace DDI.WebApi.Controllers.GL
{
    public class GeneralLedgerController<T> : ControllerBase<T> where T : class, IEntity
    {
        public GeneralLedgerController() :this(new ServiceBase<T>())
        {
        }

        public GeneralLedgerController(IService<T> serviceBase) : base(serviceBase, new DynamicTransmogrifier(), new Pagination())
        {
        }

        public IHttpActionResult GetAll(string routeName, Guid businessUnit, Guid fiscalYear, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null, UrlHelper urlHelper = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            return GetAll(routeName, businessUnit, fiscalYear, search, fields, urlHelper ?? GetUrlHelper());
        }

        public IHttpActionResult GetAll(string routeName, Guid businessUnit, Guid fiscalYear, IPageable search, string fields = null, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                return FinalizeResponse(_service.GetAll(fields, search), routeName, search, ConvertFieldList(fields, FieldsForList), urlHelper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }
    }
}