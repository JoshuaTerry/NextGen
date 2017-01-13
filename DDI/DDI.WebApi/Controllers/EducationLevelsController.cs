﻿using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class EducationLevelsController : ApiController
    {
        ServiceBase<EducationLevel> _service;

        public EducationLevelsController() : this(new ServiceBase<EducationLevel>()) { }
        internal EducationLevelsController(ServiceBase<EducationLevel> service)
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