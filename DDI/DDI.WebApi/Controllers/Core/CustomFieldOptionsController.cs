using System;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class CustomFieldOptionsController : GenericController<CustomFieldOption>
    {
        public CustomFieldOptionsController(IService<CustomFieldOption> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/customfieldoptions")]
        public IHttpActionResult GetAll()
        {
            try {
                var result = Service.GetAll();

                if (result == null)
                {
                    return NotFound();
                }
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/customfieldoptions/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            try
            {
                var result = Service.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
              
                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
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

                var response = Service.Add(item);
                return Ok();
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
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

                var response = Service.Update(id, changes);

                return Ok(response);

            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
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

                var entityToDelete = Service.GetById(id);
                if (entityToDelete == null)
                {
                    return NotFound();
                }

                var response = Service.Delete(entityToDelete.Data);

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
