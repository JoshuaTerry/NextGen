using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.GL
{
    public class AccountGroupController : GenericController<AccountGroup>
    {
        public AccountGroupController(IService<AccountGroup> serviceBase) : base(serviceBase)
        {
        }

        protected override string FieldsForAll => FieldsForList;

        [HttpGet]
        [Route("api/v1/accountgroup", Name = RouteNames.AccountGroup)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Fund, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/accountgroup/{id}", Name = RouteNames.AccountGroup + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize]
        [HttpPost]
        [Route("api/v1/accountgroup", Name = RouteNames.AccountGroup + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] AccountGroup entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize]
        [HttpPatch]
        [Route("api/v1/accountgroup/{id}", Name = RouteNames.AccountGroup + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize]
        [HttpDelete]
        [Route("api/v1/accountgroup/{id}", Name = RouteNames.AccountGroup + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
