using System.Web.Http;
using System.Web.Http.Cors; 
using DDI.Services;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionPreferencesController : ControllerBase<SectionPreference>
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
        [Route("api/v1/preferences/constituent", Name = RouteNames.SectionPreference + RouteNames.Constituent)]
        public IHttpActionResult GetSectionPreferences(string sectionName)
        {
            try
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

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper());

                return Ok(dynamicResponse);

            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }         
    }
}
