using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Web.Http;
using DDI.Services;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldOptionsController : ApiController
    {
        ServiceBase<CustomFieldOption> _service;

        public CustomFieldOptionsController()
            :this(new ServiceBase<CustomFieldOption>())
        {
        }

        internal CustomFieldOptionsController(ServiceBase<CustomFieldOption> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/customfieldoptions")]
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
        [Route("api/v1/customfieldoptions/{id}")]
        public IHttpActionResult GetById(string id)
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
        [Route("api/v1/customfieldoptions")]
        public IHttpActionResult Post([FromBody] CustomFieldOption item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/customfieldoptions/{id}")]
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
        [Route("api/v1/customfieldoptions")]
        public IHttpActionResult Delete([FromBody] CustomFieldOption entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Delete(entity);

                return Ok(response);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


    }
}
