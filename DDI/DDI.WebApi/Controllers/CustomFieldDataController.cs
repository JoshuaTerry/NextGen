using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Services;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;
using DDI.Logger;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldDataController : ApiController
    {
        ServiceBase<CustomFieldData> _service;
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AuthorizationsController));
        protected ILogger Logger => _logger;


        #region Constructors

        public CustomFieldDataController()
            :this(new ServiceBase<CustomFieldData>())
        {
        }

        internal CustomFieldDataController(ServiceBase<CustomFieldData> service)
        {
            _service = service;
        }

        #endregion Constructors

        [HttpGet]
        [Route("api/v1/customfielddata")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var result = _service.GetAll();

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/customfielddata/{id}")]
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
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/customfielddata")]
        public IHttpActionResult Post([FromBody] CustomFieldData item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = _service.Add(item);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPatch]
        [Route("api/v1/customfielddata/{id}")]
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
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpDelete]
        [Route("api/v1/customfielddata/{id}")]
        public IHttpActionResult Delete(Guid id)
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
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
