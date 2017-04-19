﻿using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.INV;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.INV
{
    public class IRSInformationController : GenericController<IRSInformation>
    {

        IRSInformationService _irsInfoService = new IRSInformationService();
        [Authorize] //(Roles = Permissions.INV_Read + "," + Permissions.Settings_Read)]
        //protected override Expression<Func<InvestmentRelationship, object>>[] GetDataIncludesForList()
        //{
        //    return new Expression<Func<InvestmentRelationship, object>>[]
        //    {
        //        e => e.Constituent,
        //        e => e.DisplayName
        //    };
        //}

        [HttpGet]
        [Route("api/v1/IRSInformations", Name = RouteNames.IRSInformation)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.IRSInformation, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/IRSInformations/{id}", Name = RouteNames.IRSInformation + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        //[Authorize] //(Roles = Permissions.INV_Read)] //add investment roles when available
        [HttpGet]
        [Route("api/v1/IRSInformations/investment/{id}", Name = RouteNames.IRSInformation + RouteNames.Investment + RouteVerbs.Get)]
        public IHttpActionResult GetByInvestmentId(Guid id)
        {
            

            try
            {
                //var search = new PageableSearch(offset, limit, orderBy);
                //var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                var response = _irsInfoService.GetIRSInformationByInvestmentId(id);
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
        //[Route("api/v1/IRSInformations", Name = RouteNames.IRSInformation + RouteVerbs.Post)]
        //public IHttpActionResult Post([FromBody] InvestmentRelationship entityToSave)
        //{
        //    return base.Post(entityToSave);
        //}

        //[Authorize] //(Roles = Permissions.INV_ReadWrite)]
        //[HttpPatch]
        //[Route("api/v1/IRSInformations/{id}", Name = RouteNames.IRSInformation + RouteVerbs.Patch)]
        //public IHttpActionResult Patch(Guid id, JObject entityChanges)
        //{
        //    return base.Patch(id, entityChanges);
        //}

        //[Authorize] //(Roles = Permissions.INV_ReadWrite)]
        //[HttpDelete]
        //[Route("api/v1/IRSInformations/{id}", Name = RouteNames.IRSInformation + RouteVerbs.Delete)]
        //public override IHttpActionResult Delete(Guid id)
        //{
        //    return base.Delete(id);
        //}

    }
}