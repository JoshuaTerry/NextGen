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
    public class ConstituentAddressesController : ApiController
    {
        private ServiceBase<ConstituentAddress> _service;
        private IPagination _pagination;
        private DynamicTransmogrifier _dynamicTransmogrifier;

        public ConstituentAddressesController()
            :this(new ServiceBase<ConstituentAddress>(), new Pagination(), new DynamicTransmogrifier())
        {
        }

        internal ConstituentAddressesController(ServiceBase<ConstituentAddress> service, IPagination pagination, DynamicTransmogrifier dynamicTransmogrifier)
        {
            _service = service;
            _pagination = pagination;
            _dynamicTransmogrifier = dynamicTransmogrifier;
        }

        [HttpGet]
        [Route("api/v1/constituentAddresses", Name = RouteNames.ConstituentAddress)]
        public IHttpActionResult GetAll()
        {
            var result = _service.GetAll(); //TODO we need to be limiting this return and do proper pagification

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Get)]
        public IHttpActionResult GetConstituentById(Guid id, string fields = null)
        {
            var constituent = _service.GetById(id);

            if (constituent == null)
            {
                return NotFound();
            }
            if (!constituent.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(constituent);
        }

        [HttpPost]
        [Route("api/v1/constituentaddresses", Name = RouteNames.ConstituentAddress + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentAddress item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
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

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Delete)]
        public IHttpActionResult Delete(Guid id)
        {
            return Ok();
        }
    }
}
