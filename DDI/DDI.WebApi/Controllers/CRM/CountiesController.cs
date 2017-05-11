﻿using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using System;
using System.Web.Http;
using DDI.Shared.Helpers;
using System.Linq.Expressions;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class CountiesController : GenericController<County>
    {
        protected override string FieldsForList => $"{nameof(County.Id)},{nameof(County.DisplayName)}";

        protected override string FieldsForSingle => new PathHelper.FieldListBuilder<County>().IncludeAll().Exclude(p => p.Cities).Include(p => p.State.DisplayName).Include(p => p.State.StateCode);

        protected override string FieldsForAll => FieldsForSingle;

        protected override Expression<Func<County, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<County, object>>[] { p => p.State };
        }

        #region Public Methods

        [HttpGet]
        [Route("api/v1/counties", Name = RouteNames.County)]
        public IHttpActionResult GetAll(Guid? stateId = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            if (stateId == null)
            {
                return base.GetAll(RouteNames.County, limit, offset, orderBy, fields);
            }

            return GetByState(stateId.Value, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/counties/state/{id}", Name = RouteNames.County + RouteNames.State)]
        [Route("api/v1/states/{id}/counties", Name = RouteNames.State + RouteNames.County)]
        public IHttpActionResult GetByState(Guid id, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {

            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var result = _service.GetAllWhereExpression(fp => fp.StateId == id, search);

                return FinalizeResponse(result, RouteNames.State + RouteNames.County, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpGet]
        [Route("api/v1/counties/{id}", Name = RouteNames.County + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        #endregion Public Methods
    }

}