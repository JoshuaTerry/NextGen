using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class AccountGroupController : GenericController<AccountGroup>
    {

        public AccountGroupController(IService<AccountGroup> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/AccountGroups/{parentgroupid}/parent")]  
        public IHttpActionResult GetByParentGroupId(Guid parentgroupid, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitMax, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var response = Service.GetAllWhereExpression(a => a.ParentGroupId == parentgroupid, search, fields);
                return FinalizeResponse(response, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("api/v1/fiscalyears/{fiscalYearId}/AccountGroups")]
        public IHttpActionResult GetAll(Guid fiscalYearId, Guid? parentGroupId = null, string fields = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var response = Service.GetAllWhereExpression(a => a.FiscalYearId == fiscalYearId && a.ParentGroupId == null, search, fields);
                return FinalizeResponse(response, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/AccountGroups/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/AccountGroups")]
        public override IHttpActionResult Post([FromBody] AccountGroup item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/AccountGroups/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/AccountGroups/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}