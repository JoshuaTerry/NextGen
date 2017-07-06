using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class SectionPreferencesController : GenericController<SectionPreference>
    {
        public SectionPreferencesController(IService<SectionPreference> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/sectionpreferences")]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/sectionpreferences/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/sectionpreferences")]
        public IHttpActionResult Post([FromBody] SectionPreference entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/sectionpreferences/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/sectionpreferences/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/sectionpreferences/{categoryName}/settings") ]
        public IHttpActionResult GetSectionPreferences(string categoryName)
        {
            try
            {
                var search = new PageableSearch(0, int.MaxValue, null);
                var response = Service.GetAllWhereExpression(s => s.SectionName == categoryName, search);
                return FinalizeResponse(response, search, null);
                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/sectionpreferences/{categoryName}/settings/{sectionName}" )]
        public IHttpActionResult GetSectionPreferenceByName(string categoryName, string sectionName)
        {
            try
            {
                var search = new PageableSearch(0, int.MaxValue, null);
                var response = Service.GetWhereExpression(s => s.SectionName == categoryName && s.Name == sectionName);
                return FinalizeResponse(response);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }
    }
}
