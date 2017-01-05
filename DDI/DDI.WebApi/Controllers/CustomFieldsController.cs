using System;
using System.Web.Http;
using DDI.Data.Models.Client.Core;
using DDI.WebApi.Services;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldsController : ApiController
    {
        GenericServiceBase<CustomField> _service;

        #region Constructors

        public CustomFieldsController() : this(new GenericServiceBase<CustomField>()) { }
        internal CustomFieldsController(GenericServiceBase<CustomField> service)
        {
            _service = service;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("api/v1/customfields")]
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

        [HttpPost]
        [Route("api/v1/customfields")]
        public IHttpActionResult Post([FromBody] CustomField item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/customfields/{id}")]
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

        #endregion
    }
}