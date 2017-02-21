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
using System.Collections.Generic;

namespace DDI.WebApi.Controllers
{
    public class PaymentMethodController : ControllerBase<PaymentMethod>
    {
        private IConstituentService ConstituentService;

        public PaymentMethodController() : this(new ConstituentService())
        {

        }

        public PaymentMethodController(IConstituentService constituentService)
        {
            ConstituentService = constituentService;
        }

        [HttpGet]
        [Route("api/v1/paymentmethods", Name = RouteNames.PaymentMethod)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.PaymentMethod + RouteNames.EFT, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/paymentmethods/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/paymentmethods", Name = RouteNames.PaymentMethod + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] PaymentMethod entityToSave)
        {
            if (entityToSave.ConstituentId.HasValue)
            {
                if (entityToSave.Constituents == null)
                {
                    entityToSave.Constituents = new List<DDI.Shared.Models.Client.CRM.Constituent>();
                }

                var constituent = ConstituentService.GetById(entityToSave.ConstituentId.Value).Data;
                
                if (constituent.PaymentMethods == null)
                {
                    constituent.PaymentMethods = new List<PaymentMethod>();
                }

                constituent.PaymentMethods.Add(entityToSave);

                return Ok(ConstituentService.Update(constituent));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("api/v1/paymentmethods/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/paymentmethods/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/paymentmethods/constituents/{id}")]
        [Route("api/v1/constituents/{id}/paymentmethods", Name = RouteNames.Constituent + RouteNames.PaymentMethod)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.PaymentMethod, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}