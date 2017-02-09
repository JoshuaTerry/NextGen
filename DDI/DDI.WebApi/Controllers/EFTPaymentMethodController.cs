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
    public class EFTPaymentMethodController : ControllerBase<EFTPaymentMethod>
    {
        [HttpGet]
        [Route("api/v1/paymentpreferences/eft", Name = RouteNames.PaymentPreference + RouteNames.EFT)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.PaymentPreference + RouteNames.EFT, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/paymentpreferences/eft/{id}", Name = RouteNames.PaymentPreference + RouteNames.EFT + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/paymentpreferences/eft", Name = RouteNames.PaymentPreference + RouteNames.EFT + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] EFTPaymentMethod entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/paymentpreferences/eft/{id}", Name = RouteNames.PaymentPreference + RouteNames.EFT + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/paymentpreferences/eft/{id}", Name = RouteNames.PaymentPreference + RouteNames.EFT + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/paymentpreferences/eft/constituents/{id}")]
        [Route("api/v1/constituents/{id}/paymentpreferences/eft", Name = RouteNames.Constituent + RouteNames.PaymentPreference + RouteNames.EFT)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.PaymentPreference + RouteNames.EFT, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}