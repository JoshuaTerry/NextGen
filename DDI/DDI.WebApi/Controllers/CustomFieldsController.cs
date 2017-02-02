using System;
using System.Web.Http;
using DDI.Shared.Models.Client.Core;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldsController : ControllerBase<CustomField>
    {
        [HttpGet]
        [Route("api/v1/customfields", Name = RouteNames.CustomField)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.CustomField, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/customfields/{id}")]
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

        [HttpGet]
        [Route("api/v1/customfields/entity/{entityId}")]
        public IHttpActionResult GetByEntityId(int entityId)
        {
            var result = _service.GetByEntityId(entityId);

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
        [Route("api/v1/customfields", Name = RouteNames.CustomField + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] CustomField item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/customfields/{id}", Name = RouteNames.CustomField + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
        [HttpDelete]
        [Route("api/v1/customfields")]
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