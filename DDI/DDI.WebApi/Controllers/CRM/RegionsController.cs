﻿using DDI.Services.Search;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class RegionsController : GenericController<Region>
    {

        [HttpGet]
        [Route("api/v1/regions/regionlevels/{level}")]
        [Route("api/v1/regionlevels/{level}/regions", Name = RouteNames.RegionLevel + RouteNames.Region)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByLevel(int level, string fields = null, int? offset = null, int? limit = 1000, string orderBy = OrderByProperties.DisplayName)
        {
            return GetRegions(level, null, fields, offset, limit, orderBy);
        }

        [HttpGet]
        [Route("api/v1/regions/{id}/regionlevels/{nextLevel}")]
        [Route("api/v1/regionlevels/{nextLevel}/regions/{id}", Name = RouteNames.Region + RouteNames.RegionLevel + RouteNames.RegionChildren)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByNextLevel(int nextLevel, Guid? id = null, string fields = null, int? offset = null, int? limit = 1000, string orderBy = OrderByProperties.DisplayName)
        {
            return GetRegions(nextLevel, id, fields, offset, limit, orderBy);
        }

        private IHttpActionResult GetRegions(int level, Guid? id, string fields, int? offset, int? limit, string orderBy)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Level == level && (id == null || a.ParentRegionId == id), search);
                return FinalizeResponse(response, RouteNames.RegionLevel + RouteNames.Region, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/regions", Name = RouteNames.Region)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ParentRegionId == null, search);
                return FinalizeResponse(response, RouteNames.Region, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/regions", Name = RouteNames.Region + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Region entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/regions/{id}", Name = RouteNames.Region + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
