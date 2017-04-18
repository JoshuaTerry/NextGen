using DDI.Services.Search;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class TagsController : GenericController<Tag>
    {
        [HttpGet]
        [Route("api/v1/tags")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.Order, string fields = null)
        {
            return base.GetAll(null, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/tags/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/tags")]
        public IHttpActionResult Post([FromBody] Tag entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/tags/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/tags/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/tags/constituents/{id}")]
        [Route("api/v1/constituents/{id}/tags", Name = RouteNames.Constituent + RouteNames.Tag + RouteVerbs.Get)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.Order)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search);
                return FinalizeResponse(response, "", search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/tags/constituenttypes/{id}")]
        [Route("api/v1/constituenttypes/{id}/tags", Name = RouteNames.ConstituentType + RouteNames.Tag + RouteVerbs.Get)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentTypeId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.Order)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ConstituentTypes.Any(c => c.Id == id), search);
                return FinalizeResponse(response, "", search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/tags/taggroups/{id}")]
        [Route("api/v1/taggroups/{id}/tags")]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByTagGroupId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.Order)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.TagGroupId == id, search);
                return FinalizeResponse(response, "", search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }
    }
}