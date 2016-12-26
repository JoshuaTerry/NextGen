﻿using System.Web.Http;
using DDI.Data.Models.Client.CRM;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class EducationLevelsController : ApiController
    {
        GenericServiceBase<EducationLevel> _service;

        public EducationLevelsController() : this(new GenericServiceBase<EducationLevel>()) { }
        internal EducationLevelsController(GenericServiceBase<EducationLevel> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/educationlevels")]
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