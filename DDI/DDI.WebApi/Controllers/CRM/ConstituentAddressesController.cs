using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class ConstituentAddressesController : GenericController<ConstituentAddress>
    {
        public ConstituentAddressesController(ConstituentAddressService service) : base(service) { }

        protected override Expression<Func<ConstituentAddress, object>>[] GetDataIncludesForSingle()
        {
            return DataIncludes();
        }

        protected override Expression<Func<ConstituentAddress, object>>[] GetDataIncludesForList()
        {
            return DataIncludes();
        }

        private Expression<Func<ConstituentAddress, object>>[] DataIncludes()
        {
            Expression<Func<ConstituentAddress, object>>[] includes =
            {
                a => a.Address,
                a => a.AddressType,
                a => a.Address.Region1,
                a => a.Address.Region2,
                a => a.Address.Region3,
                a => a.Address.Region4
            };
            return includes;
        }

        [HttpGet]
        [Route("api/v1/constituentAddresses")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitDefault, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/constituentaddresses/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/constituentaddresses")]
        public override IHttpActionResult Post([FromBody] ConstituentAddress item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/constituentaddresses/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/constituentaddresses/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/constituentaddresses/constituents/{id}")]
        [Route("api/v1/constituents/{id}/constituentaddresses")]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search, fields);
                return FinalizeResponse(response, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
