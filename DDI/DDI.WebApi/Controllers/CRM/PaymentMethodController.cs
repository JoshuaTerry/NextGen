using DDI.Services;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DDI.Shared.Helpers;
using System.Linq.Expressions;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class PaymentMethodController : GenericController<PaymentMethod>
    {
        private IConstituentService ConstituentService;

        public PaymentMethodController() : this(new PaymentMethodService(), new ConstituentService())
        {

        }

        public PaymentMethodController(PaymentMethodService service, IConstituentService constituentService) : base(service)
        {            
            ConstituentService = constituentService;
        }

        protected override string FieldsForList => $"{nameof(PaymentMethod.Id)},{nameof(PaymentMethod.DisplayName)}";

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Constituents).Include(p => p.EFTFormat.DisplayName);

        protected override Expression<Func<PaymentMethod, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<PaymentMethod, object>>[] { p => p.EFTFormat };
        }

        [HttpGet]
        [Route("api/v1/paymentmethods", Name = RouteNames.PaymentMethod)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.PaymentMethod, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/paymentmethods/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/paymentmethods", Name = RouteNames.PaymentMethod + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] PaymentMethod entityToSave)
        {
            try
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
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/paymentmethods/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/paymentmethods/{id}", Name = RouteNames.PaymentMethod + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [Authorize(Roles = Permissions.CRM_Read)]
        [HttpGet]
        [Route("api/v1/paymentmethods/constituents/{id}")]
        [Route("api/v1/constituents/{id}/paymentmethods", Name = RouteNames.Constituent + RouteNames.PaymentMethod)]
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.PaymentMethod, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}