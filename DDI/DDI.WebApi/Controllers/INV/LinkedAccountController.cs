using DDI.Services;
using DDI.Shared.Models.Client.INV;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.INV
{
    public class LinkedAccountController : GenericController<LinkedAccount>
    {
        private LinkedAccountService _laService;

        public LinkedAccountController(LinkedAccountService service) : base(service)
        {
            _laService = service;
        }
        
        [Authorize] //(Roles = Permissions.INV_Read + "," + Permissions.Settings_Read)]
        
        [HttpGet]
        [Route("api/v1/linkedaccounts")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/linkedaccounts/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            try
            {
                //var search = new PageableSearch(offset, limit, orderBy);
                //var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                var response = _laService.GetLinkedAccountById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
            //return base.GetById(id, fields);
        }

        //[Authorize] //(Roles = Permissions.INV_Read)] //add investment roles when available
        [HttpGet]
        [Route("api/v1/linkedaccounts/investment/{id}")]
        public IHttpActionResult GetByInvestmentId(Guid id)
        {
            

            try
            {
                //var search = new PageableSearch(offset, limit, orderBy);
                //var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                var response = _laService.GetLinkedAccountByInvestmentId(id);
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
        [Route("api/v1/linkedaccounts")]
        public IHttpActionResult Post([FromBody] LinkedAccount entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/linkedaccounts/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/linkedaccounts/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}