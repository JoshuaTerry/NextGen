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
            return base.GetAll(RouteNames.PaymentPreference, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/paymentpreferences/{id}", Name = RouteNames.PaymentPreference + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/paymentpreferences", Name = RouteNames.PaymentPreference + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] PaymentMethodBase entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/paymentpreferences/{id}", Name = RouteNames.PaymentPreference + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/paymentpreferences/{id}", Name = RouteNames.PaymentPreference + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/paymentpreferences/constituents/{id}")]
        [Route("api/v1/constituents/{id}/paymentpreferences", Name = RouteNames.Constituent + RouteNames.PaymentPreference)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = null, int? limit = 25, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.PaymentPreference, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}