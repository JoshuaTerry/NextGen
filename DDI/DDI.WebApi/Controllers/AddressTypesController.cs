using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using LinqKit;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class AddressTypesController : ControllerBase<AddressType>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/addresstypes", Name = RouteNames.AddressType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.AddressType, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/addresstypes/{id}", Name = RouteNames.AddressType + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/addresstypes", Name = RouteNames.AddressType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] AddressType entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/addresstypes/{id}", Name = RouteNames.AddressType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/addresstypes/{id}", Name = RouteNames.AddressType + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}