using System;
using System.Linq;
using System.Web.Http;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CP;

namespace DDI.WebApi.Controllers
{
    public class CardPaymentMethodController : ControllerBase<CardPaymentMethod>
    {
        [HttpGet]
        [Route("api/v1/paymentpreferences/card", Name = RouteNames.PaymentPreference + RouteNames.Card)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.PaymentPreference + RouteNames.Card, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/paymentpreferences/card/{id}", Name = RouteNames.PaymentPreference + RouteNames.Card + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/paymentpreferences/card", Name = RouteNames.PaymentPreference + RouteNames.Card + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] CardPaymentMethod entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/paymentpreferences/card/{id}", Name = RouteNames.PaymentPreference + RouteNames.Card + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/paymentpreferences/card/{id}", Name = RouteNames.PaymentPreference + RouteNames.Card + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/paymentpreferences/card/constituents/{id}")]
        [Route("api/v1/constituents/{id}/paymentpreferences/card", Name = RouteNames.Constituent + RouteNames.PaymentPreference + RouteNames.Card)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.PaymentPreference + RouteNames.Card, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}