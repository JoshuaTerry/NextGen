using DDI.Shared;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CP
{
    [Authorize]
    public class PaymentMethodController : GenericController<PaymentMethod>
    {
        public PaymentMethodController(IService<PaymentMethod> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/paymentmethod", Name = RouteNames.PaymentMethod)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.PaymentMethod, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/paymentmethod/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/paymentmethod", Name = RouteNames.PaymentMethod + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] PaymentMethod entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/paymentmethod/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/paymentmethod/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}