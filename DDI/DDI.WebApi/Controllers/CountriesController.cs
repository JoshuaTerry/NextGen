using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Web.Http;
using DDI.Shared.Models.Common;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System.Linq;
using DDI.Shared.Logger;

namespace DDI.WebApi.Controllers
{
    public class CountriesController : ControllerBase<Country>
    {
        private static readonly Logger _logger = Logger.GetLogger(typeof(CountriesController));
        private const string US = "US";
        [HttpGet]
        [Route("api/v1/countries", Name = RouteNames.Country)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var urlHelper = GetUrlHelper();
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            try
            {
                var response = base.Service.GetAll(search);
                if (response.Data == null)
                {
                    return NotFound();
                }
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var totalCount = response.TotalResults;

                var us = response.Data.FirstOrDefault(c => c.CountryCode == US);
                if (us != null)
                {
                    int index = response.Data.IndexOf(us);
                    response.Data.RemoveAt(index);
                    response.Data.Insert(0, us);
                }

                Pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, RouteNames.Country);
                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, urlHelper, fields);

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/countries", Name = RouteNames.Country + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Country item)
        {
            return base.Post(GetUrlHelper(), item);
        }

        [HttpPatch]
        [Route("api/v1/countries/{id}", Name = RouteNames.Country + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(GetUrlHelper(), id, changes);
        }
    }
}
