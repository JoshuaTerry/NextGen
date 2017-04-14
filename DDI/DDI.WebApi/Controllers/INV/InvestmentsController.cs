﻿using DDI.Data;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.INV;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.INV
{
    

    public class InvestmentsController : GenericController<Investment>
    {
        InvestmentService _invService = new InvestmentService();

        [Authorize] //(Roles = Permissions.INV_Read + "," + Permissions.Settings_Read)]
        //protected override Expression<Func<Investment, object>>[] GetDataIncludesForList()
        //{
        //    return new Expression<Func<Investment, object>>[]
        //    {
        //        e => e.Constituent,
        //        e => e.DisplayName
        //    };
        //}

        [HttpGet]
        [Route("api/v1/investments", Name = RouteNames.Investment)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Investment, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/investments/{id}", Name = RouteNames.Investment + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {

            //return base.GetById(id, fields);

            try
            {
                //var search = new PageableSearch(offset, limit, orderBy);
                //var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                var response = _invService.GetInvestmentById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        
        //[Authorize] //(Roles = Permissions.INV_ReadWrite)]
        //[HttpPost]
        //[Route("api/v1/investments", Name = RouteNames.Investment + RouteVerbs.Post)]
        //public IHttpActionResult Post([FromBody] Investment entityToSave)
        //{
        //    return base.Post(entityToSave);
        //}

        //[Authorize] //(Roles = Permissions.INV_ReadWrite)]
        //[HttpPatch]
        //[Route("api/v1/investments/{id}", Name = RouteNames.Investment + RouteVerbs.Patch)]
        //public IHttpActionResult Patch(Guid id, JObject entityChanges)
        //{
        //    return base.Patch(id, entityChanges);
        //}

        //[Authorize] //(Roles = Permissions.INV_ReadWrite)]
        //[HttpDelete]
        //[Route("api/v1/investments/{id}", Name = RouteNames.Investment + RouteVerbs.Delete)]
        //public override IHttpActionResult Delete(Guid id)
        //{
        //    return base.Delete(id);
        //}

    }
}