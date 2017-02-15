using System;
using System.Web.Http;
using DDI.Shared.Models.Client.Core;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
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
        [Route("api/v1/sectionpreferences", Name = RouteNames.SectionPreference)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.SectionPreference, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/sectionpreferences/{id}", Name = RouteNames.SectionPreference + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/sectionpreferences", Name = RouteNames.SectionPreference + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] SectionPreference entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/sectionpreferences/{id}", Name = RouteNames.SectionPreference + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/sectionpreferences/{id}", Name = RouteNames.SectionPreference + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/sectionpreferences/{sectionname}", Name = RouteNames.SectionPreference + RouteNames.SectionName)]
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
