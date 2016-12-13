using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Business.Services;
using DDI.Data.Models.Common;
using DDI.Data.Models.Client;

namespace DDI.Business.Controllers
{
    public class ClergyTypesController : ApiController
    {
        GenericServiceBase<ClergyType> _service;

        public ClergyTypesController() : this(new GenericServiceBase<ClergyType>()) { }
        internal ClergyTypesController(GenericServiceBase<ClergyType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/clergytypes")]
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
    }
}