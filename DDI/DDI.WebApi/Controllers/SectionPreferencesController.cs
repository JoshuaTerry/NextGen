using System.Web.Http;
using System.Web.Http.Cors; 
using DDI.Services; 
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionPreferencesController : ApiController
    {
        private ISectionPreferenceService _service;
        private IPagination _pagination;
        private DynamicTransmogrifier _dynamicTransmogrifier;

        public SectionPreferencesController()
            :this(new SectionPreferenceService(), new Pagination(), new DynamicTransmogrifier())
        {
        }

        internal SectionPreferencesController(ISectionPreferenceService service, IPagination pagination, DynamicTransmogrifier dynamicTransmogrifier)
        {
            _service = service;
            _pagination = pagination;
            _dynamicTransmogrifier = dynamicTransmogrifier;
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
