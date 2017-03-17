﻿using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Services;
using DDI.Services.Extensions;
using Newtonsoft.Json.Linq;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using DDI.Services.ServiceInterfaces;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class ConstituentAddressesController : ControllerBase<ConstituentAddress>
    {

        public ConstituentAddressesController()
            :base(new ConstituentAddressService())
        {
            
        }

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
            Expression<Func<ConstituentAddress, object>>[] includes = {a => a.Address, a => a.AddressType};
            return includes;
        }

        [HttpGet]
        [Route("api/v1/constituentAddresses", Name = RouteNames.ConstituentAddress)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitDefault, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ConstituentAddress, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/constituentaddresses", Name = RouteNames.ConstituentAddress + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentAddress item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/constituentaddresses/constituents/{id}")]
        [Route("api/v1/constituents/{id}/constituentaddresses", Name = RouteNames.Constituent + RouteNames.ConstituentAddress)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.ConstituentAddress, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
