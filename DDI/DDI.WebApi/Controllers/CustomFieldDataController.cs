using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Services;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldDataController : ApiController
    {
        ServiceBase<CustomFieldData> _service;

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
        [Route("api/v1/customfielddata/{id}")]
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
        [Route("api/v1/customfielddata")]
        public IHttpActionResult Post([FromBody] CustomFieldData item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);

            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] List<CustomFieldData> items)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    items.ForEach(i => _service.Add(i));
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
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
                return BadRequest(ex.ToString());
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
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
