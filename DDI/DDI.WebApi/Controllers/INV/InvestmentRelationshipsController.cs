using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.INV;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared;

namespace DDI.WebApi.Controllers.INV
{
    public class InvestmentRelationshipsController : GenericController<InvestmentRelationship>
    {

        InvestmentRelationshipService _invrelService;

        public InvestmentRelationshipsController() : base(Factory.CreateService<InvestmentRelationshipService>())
        {
            _invrelService = (InvestmentRelationshipService)Service;
        }

        [Authorize] //(Roles = Permissions.INV_Read + "," + Permissions.Settings_Read)]

        [HttpGet]
        [Route("api/v1/investmentrelationships", Name = RouteNames.InvestmentRelationship)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.InvestmentRelationship, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/investmentrelationships/{id}", Name = RouteNames.InvestmentRelationship + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        //[Authorize] //(Roles = Permissions.INV_Read)] //add investment roles when available
        [HttpGet]
        [Route("api/v1/investmentrelationships/constituent/{id}", Name = RouteNames.InvestmentRelationship + RouteNames.Constituent + RouteVerbs.Get)]
        public IHttpActionResult GetByConstituentId(Guid id)
        {
            

            try
            {
                //var search = new PageableSearch(offset, limit, orderBy);
                //var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                var response = _invrelService.GetInvestmentByConstituentId(id);
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
        [Route("api/v1/investmentrelationships", Name = RouteNames.InvestmentRelationship + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] InvestmentRelationship entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/investmentrelationships/{id}", Name = RouteNames.InvestmentRelationship + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/investmentrelationships/{id}", Name = RouteNames.InvestmentRelationship + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}