using DDI.Services;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using DDI.Services.Search;

namespace DDI.WebApi.Controllers
{
    public class RegionsController : ControllerBase<Region>
    {
        protected new IRegionService Service => (IRegionService) base.Service;

        public RegionsController()
            :base(new RegionService())
        {
        }

        [HttpGet]
        [Route("api/v1/regions/{level}/{id}", Name = RouteNames.Region + RouteNames.Level)]
        public IHttpActionResult GetRegionsByLevel(Guid? id, int level)
        {
            try
            {
                var result = Service.GetRegionsByLevel(id, level);
                return FinalizeResponse(result, RouteNames.Region + RouteNames.Level, null);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("api/v1/regions/address", Name = RouteNames.Region + RouteNames.Address)]
        public IHttpActionResult GetRegionsByAddress(Guid? countryid, Guid? stateId, Guid? countyId, string city, string zipcode)
        {
            try
            {
                var result = Service.GetRegionsByAddress(countryid, stateId, countyId, city, zipcode);
                return FinalizeResponse(result, RouteNames.Region + RouteNames.Address, null);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/regions", Name = RouteNames.Region)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ParentRegionId == null, search);
                return FinalizeResponse(response, RouteNames.Region, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/regions", Name = RouteNames.Region + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Region entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
