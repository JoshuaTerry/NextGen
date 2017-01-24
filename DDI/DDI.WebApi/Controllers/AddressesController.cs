using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Services;
using DDI.Services.Extensions;
using Newtonsoft.Json.Linq;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class AddressesController : ControllerBase
    {
        private ServiceBase<Address> _service;

        public AddressesController()
            :this(new ServiceBase<Address>())
        {
        }

        internal AddressesController(ServiceBase<Address> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/addresses", Name = RouteNames.Address)]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderby = "DisplayName", string fields = null)
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

                var urlHelper = GetUrlHelper();
                var totalCount = result.TotalResults;

                Pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, RouteNames.Address);
                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, urlHelper, fields);

                return Ok(dynamicResult);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Get)]
        public IHttpActionResult GetAddressById(Guid id, string fields = null)
        {
            try
            {
                var address = _service.GetById(id);

                if (address == null)
                {
                    return NotFound();
                }
                if (!address.IsSuccessful)
                {
                    return InternalServerError();
                }

                var dynamicAddress = DynamicTransmogrifier.ToDynamicResponse(address, GetUrlHelper(), fields);

                return Ok(dynamicAddress);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/addresses", Name = RouteNames.Address + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Address item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Add(item);
                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper());
                return Ok(dynamicResponse);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPatch]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);
                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper());

                return Ok(dynamicResponse);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Delete)]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                var entity = _service.GetById(id);
                if (entity == null)
                {
                    return NotFound();
                }
                _service.Delete(entity.Data);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
