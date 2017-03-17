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
using DDI.Logger;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldOptionsController : ControllerBase<CustomFieldOption>
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
            try {
                var result = _service.GetAll();

                if (result == null)
                {
                    return NotFound();
                }
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.Message);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/customfieldoptions/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            try
            {
                var result = _service.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
              
                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.Message);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/customfieldoptions")]
        public IHttpActionResult Post([FromBody] CustomFieldOption item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Add(item);
                return Ok();
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.Message);
                return InternalServerError(new Exception(ex.Message));
            }
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
                base.Logger.LogError(ex.Message);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpDelete]
        [Route("api/v1/customfieldoptions/{id}")]
        public override IHttpActionResult  Delete(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entityToDelete = _service.GetById(id);
                if (entityToDelete == null)
                {
                    return NotFound();
                }

                var response = _service.Delete(entityToDelete.Data);

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.Message);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
