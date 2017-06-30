﻿using DDI.Services;
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
    public class InvestmentIRSInformationController : GenericController<InvestmentIRSInformation>
    {

        private InvestmentIRSInformationService _irsInfoService;

        public InvestmentIRSInformationController(InvestmentIRSInformationService service) : base(service)
        {
            _irsInfoService = service;
        }

        [Authorize] //(Roles = Permissions.INV_Read + "," + Permissions.Settings_Read)]

        [HttpGet]
        [Route("api/v1/investmentirsinformations")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/investmentirsinformations/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        //[Authorize] //(Roles = Permissions.INV_Read)] //add investment roles when available
        [HttpGet]
        [Route("api/v1/investmentirsinformations/investment/{id}")]
        public IHttpActionResult GetByInvestmentId(Guid id)
        {
            

            try
            {
                //var search = new PageableSearch(offset, limit, orderBy);
                //var response = Service.GetAllWhereExpression(a => a.InvestmentId == id, search);
                var response = _irsInfoService.GetIRSInformationByInvestmentId(id);
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
        [Route("api/v1/investmentirsinformations")]
        public IHttpActionResult Post([FromBody] InvestmentIRSInformation entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/investmentirsinformations/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/investmentirsinformations/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}