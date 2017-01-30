using System;
using System.Web.Http;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CP;

namespace DDI.WebApi.Controllers
{
    public class PaymentPreferencesController : ControllerBase<PaymentMethodBase>
    {
        [HttpGet]
        [Route("api/v1/paymentpreferences", Name = RouteNames.PaymentPreference)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.PaymentPreference, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/paymentpreferences/{id}", Name = RouteNames.PaymentPreference + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(GetUrlHelper(), id, fields);
        }

        [HttpPost]
        [Route("api/v1/paymentpreferences", Name = RouteNames.PaymentPreference + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] PaymentMethodBase entityToSave)
        {
            return base.Post(GetUrlHelper(), entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/paymentpreferences/{id}", Name = RouteNames.PaymentPreference + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(GetUrlHelper(), id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/paymentpreferences/{id}", Name = RouteNames.PaymentPreference + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}