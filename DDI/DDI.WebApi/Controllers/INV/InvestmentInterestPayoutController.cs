using DDI.Services;
using DDI.Shared.Models.Client.INV;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.INV
{
    public class InvestmentInterestPayoutController : GenericController<InvestmentInterestPayout>
    {
        private InvestmentInterestPayoutService _invpayService;

        public InvestmentInterestPayoutController(InvestmentInterestPayoutService service) : base(service)
        {
            _invpayService = service;
        }

        [Authorize] //(Roles = Permissions.INV_Read + "," + Permissions.Settings_Read)]

        [HttpGet]
        [Route("api/v1/investmentinterestpayouts")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/investmentinterestpayouts/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            try
            {
                var response = _invpayService.GetInterestPayoutById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            };
        }

        //[Authorize] //(Roles = Permissions.INV_Read)] //add investment roles when available
        [HttpGet]
        [Route("api/v1/investmentinterestpayouts/investment/{id}")]
        public IHttpActionResult GetByInvestmentId(Guid id)
        {
            try
            {
                var response = _invpayService.GetInterestPayoutByInvestmentId(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpPost]
        [Route("api/v1/investmentinterestpayouts")]
        public override IHttpActionResult Post([FromBody] InvestmentInterestPayout entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/investmentinterestpayouts/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/investmentinterestpayouts/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}