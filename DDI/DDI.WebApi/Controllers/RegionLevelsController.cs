using System;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class RegionLevelsController : ApiController
    {
        ServiceBase<RegionLevel> _service;

        public RegionLevelsController() : this(new ServiceBase<RegionLevel>()) { }
        internal RegionLevelsController(ServiceBase<RegionLevel> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/regionlevels")]
        public IHttpActionResult GetAll()
        {
            var result = _service.GetAll();

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
        [Route("api/v1/regionlevels/{id}")]
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
        [Route("api/v1/regionlevels")]
        public IHttpActionResult Post([FromBody] RegionLevel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/regionlevels/{id}")]
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
        [Route("api/v1/regionlevels")]
        public IHttpActionResult Delete(RegionLevel item)
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
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
