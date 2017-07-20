using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class CountiesController : GenericController<County>
    {
        public CountiesController(IService<County> service) : base(service) { }

        protected override string FieldsForList => $"{nameof(County.Id)},{nameof(County.DisplayName)}";

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Cities).Include(p => p.State.DisplayName).Include(p => p.State.StateCode);

        protected override Expression<Func<County, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<County, object>>[] { p => p.State };
        }

        #region Public Methods

        [HttpGet]
        [Route("api/v1/counties")]
        public IHttpActionResult GetAll(Guid? stateId = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            if (stateId == null)
            {
                return base.GetAll(limit, offset, orderBy, fields);
            }

            return GetByState(stateId.Value, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/counties/state/{id}")]
        [Route("api/v1/states/{id}/counties")]
        public IHttpActionResult GetByState(Guid id, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {

            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(fp => fp.StateId == id, search, fields);

                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpGet]
        [Route("api/v1/counties/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        #endregion Public Methods
    }

}