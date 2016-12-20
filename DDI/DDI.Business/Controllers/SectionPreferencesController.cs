﻿using System.Web.Http;
using System.Web.Http.Cors;
using DDI.Business.Services;

namespace DDI.Business.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionPreferencesController : ApiController
    {
        private ISectionPreferenceService _service;

        public SectionPreferencesController()
            :this(new SectionPreferenceService())
        {
        }

        internal SectionPreferencesController(ISectionPreferenceService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/preferences/constituent")]
        public IHttpActionResult GetSectionPreferences(string sectionName)
        {
            var response = _service.GetPreferencesBySectionName(sectionName);
            
            if (response == null)
            {
                return NotFound();
            }
            if (!response.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(response);
        }         
    }
}