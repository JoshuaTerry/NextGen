﻿using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Routing;

namespace DDI.WebApi.Controllers.GL
{

    [Authorize]
    public class LedgerController : GenericController<Ledger>
    {
        public LedgerController(IService<Ledger> service) : base(service) { }

        private const string ROUTENAME_GETBYUNIT = RouteNames.BusinessUnit + RouteNames.Ledger + RouteVerbs.Get;
        
        protected override Expression<Func<Ledger, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Ledger, object>>[]
            {
                c => c.SegmentLevels,
            };
        }

        protected override string FieldsForList => $"{nameof(Ledger.Id)},{nameof(Ledger.Code)},{nameof(Ledger.DisplayName)}";

        protected override string FieldsForAll => FieldListBuilder
            .IncludeAll()
            .Exclude(p => p.BusinessUnit)
            .Exclude(p => p.LedgerAccounts)
            .Exclude(p => p.OrgLedger)
            .Exclude(p => p.FiscalYears)
            .Exclude(p => p.SegmentLevels.First().Ledger)
            .Exclude(p => p.SegmentLevels.First().Segments)
            .Include(p => p.DefaultFiscalYear.Name)
            ;


        [HttpGet]
        [Route("api/v1/ledgers/{id}", Name = RouteNames.Ledger + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/ledgers/businessunit/{buid}", Name = RouteNames.Ledger + RouteNames.BusinessUnit + RouteVerbs.Get)]
        [Route("api/v1/businessunits/{buid}/ledgers", Name = ROUTENAME_GETBYUNIT)]
        public IHttpActionResult GetByBusinessUnit(Guid buid, string fields = null)
        {
            try
            {
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(l => l.BusinessUnitId == buid, null, fields);
                return FinalizeResponse(result, ROUTENAME_GETBYUNIT, null, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/ledgers", Name = RouteNames.Ledger + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Ledger item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/ledgers/{id}", Name = RouteNames.Ledger + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/ledgers/{id}", Name = RouteNames.Ledger + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}