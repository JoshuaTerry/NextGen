using System;
using System.Web.Http;
using DDI.Shared.Models.Client.Core;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldDataController : ControllerBase<CustomFieldData>
    {
        [HttpGet]
        [Route("api/v1/customfielddata")]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return Service.GetAll(RouteNames.CustomFieldData, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/customfielddata/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return Service.GetById(id, fields);
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
