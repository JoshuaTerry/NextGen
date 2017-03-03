using System;
using System.Web.Http;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class SchoolsController : ControllerBase<School>
    {
        private string _defaultFields = null;

        [HttpGet]
        [Route("api/v1/schools", Name = RouteNames.School)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.School, limit, offset, orderBy, ConvertFieldList(fields));
        }

        [HttpGet]
        [Route("api/v1/schools/{id}", Name = RouteNames.School + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = "all")
        {
            return base.GetById(id, ConvertFieldList(fields));
        }

        [HttpPost]
        [Route("api/v1/schools", Name = RouteNames.School + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] School entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/schools/{id}", Name = RouteNames.School + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/schools/{id}", Name = RouteNames.School + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        private string ConvertFieldList(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                if (_defaultFields == null)
                {
                    _defaultFields = new PathHelper.FieldListBuilder<School>()
                        .Include(p => p.Id)
                        .Include(p => p.DisplayName)
                        .Include(p => p.IsActive);
                }
                return _defaultFields;
            }
            else if (string.Compare(fields, "all", true) == 0)
            {
                return string.Empty;
            }

            return fields;
        }


    }
}
