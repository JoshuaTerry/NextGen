using DDI.Services;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class RegionsController : ApiController
    {
        private IRegionService _service;
        private IPagination _pagination;
        private DynamicTransmogrifier _dynamicTransmogrifier;

        public RegionsController()
            :this(new RegionService(), new Pagination(), new DynamicTransmogrifier())
        {
        }

        internal RegionsController(IRegionService service, IPagination pagination, DynamicTransmogrifier dynamicTransmogrifier)
        {
            _service = service;
            _pagination = pagination;
            _dynamicTransmogrifier = dynamicTransmogrifier;
        }
                

        [HttpGet]
        [Route("api/v1/regions/{level}/{id}", Name = RouteNames.Region + RouteVerbs.Get)]
        public IHttpActionResult GetRegionsByLevel(Guid? id, int level)
        {
            var result = _service.GetRegionsByLevel(id, level);

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
        [Route("api/v1/regions/address", Name = RouteNames.Region + RouteNames.Address + RouteVerbs.Get)]
        public IHttpActionResult GetRegionsByAddress(Guid? countryid, Guid? stateId, Guid? countyId, string city, string zipcode)
        {
            var result = _service.GetRegionsByAddress(countryid, stateId, countyId, city, zipcode);

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
        [Route("api/v1/regions/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            var result = _service.GetById(id);

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

        [HttpPost]
        [Route("api/v1/regions", Name = RouteNames.Region + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Region item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Patch)]
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
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Delete)]
        public IHttpActionResult Delete(Region item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Delete(item);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
