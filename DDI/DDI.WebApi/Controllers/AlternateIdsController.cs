using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using DDI.Services.ServiceInterfaces;
using DDI.Services.Services;

namespace DDI.WebApi.Controllers
{
    public class AlternateIdsController : ControllerBase<AlternateId>
    {
        private IAlternateIdService _service;
        public AlternateIdsController() :this(new AlternateIdService()) { }

        public AlternateIdsController(IAlternateIdService service)
        {
            this._service = service;
        }
        [HttpGet]
        [Route("api/v1/alternateid", Name = RouteNames.AlternateId)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.AlternateId, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/alternateid/{id}", Name = RouteNames.AlternateId + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(GetUrlHelper(), id, fields);
        }

        //[HttpGet]
        //[Route("api/v1/alternateid/constituent/{id}", Name = RouteNames.AlternateId + RouteVerbs.Get)]
        //public IHttpActionResult GetByConstituentId(Guid constituentId, string fields = null)
        //{
        //    try
        //    {
        //        var response = _service.GetAlternateIdsByConstituent(constituentId);
        //        if (response.Data == null)
        //        {
        //            return NotFound();
        //        }
        //        if (!response.IsSuccessful)
        //        {
        //            return BadRequest(response.ErrorMessages.ToString());
        //        }

        //        var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper(), fields);

        //        return Ok(dynamicResponse);
        //    }
        //    catch (Exception)
        //    {
        //        return InternalServerError();
        //    }
        //}

        [HttpPost]
        [Route("api/v1/alternateid", Name = RouteNames.AlternateId + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] AlternateId entityToSave)
        {
            return base.Post(GetUrlHelper(), entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/alternateid/{id}", Name = RouteNames.AlternateId + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(GetUrlHelper(), id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/alternateid/{id}", Name = RouteNames.AlternateId + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}