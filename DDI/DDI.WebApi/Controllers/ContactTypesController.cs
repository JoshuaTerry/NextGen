﻿using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    public class ContactTypesController : ControllerBase
    {
        ServiceBase<ContactType> _service;

        public ContactTypesController() : this(new ServiceBase<ContactType>()) { }
        internal ContactTypesController(ServiceBase<ContactType> service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("api/v1/contacttypes", Name = RouteNames.ContactType)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderby
            };

            try
            {
                var result = _service.GetAll(search);

                if (result == null)
                {
                    return NotFound();
                }
                if (!result.IsSuccessful)
                {
                    return InternalServerError();
                }

                var totalCount = result.TotalResults;

                Pagination.AddPaginationHeaderToResponse(GetUrlHelper(), search, totalCount, RouteNames.ContactType);
                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);

                return Ok(dynamicResult);

            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }
    }
}