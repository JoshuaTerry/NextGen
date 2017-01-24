using System;
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
    public class ConstituentAddressesController : ControllerBase
    {
        private ServiceBase<ConstituentAddress> _service;

        public ConstituentAddressesController()
            :this(new ServiceBase<ConstituentAddress>())
        {
        }

        internal ConstituentAddressesController(ServiceBase<ConstituentAddress> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituentAddresses", Name = RouteNames.ConstituentAddress)]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderby = null, string fields = null)
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

                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);

                return Ok(dynamicResult);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Get)]
        public IHttpActionResult GetConstituentAddressById(Guid id, string fields = null)
        {
            try
            {
                var constituentAddress = _service.GetById(id);

                if (constituentAddress == null)
                {
                    return NotFound();
                }
                if (!constituentAddress.IsSuccessful)
                {
                    return InternalServerError();
                }

                var dynamicConstituentAddress = DynamicTransmogrifier.ToDynamicResponse(constituentAddress, GetUrlHelper());

                return Ok(dynamicConstituentAddress);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/constituentaddresses", Name = RouteNames.ConstituentAddress + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentAddress item)
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
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Patch)]
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
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Delete)]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                var entity = _service.GetById(id).Data;
                if (entity == null)
                {
                    return NotFound();
                }
                _service.Delete(entity);

                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
