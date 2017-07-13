using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class MaritalStatusesController : GenericController<MaritalStatus>
    {
        public MaritalStatusesController(IService<MaritalStatus> service) : base(service) { }

        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/maritalstatuses")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }
    }
}