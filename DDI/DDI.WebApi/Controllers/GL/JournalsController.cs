using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Routing;
using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.GL
{

    //[Authorize]
    public class JournalsController : GenericController<Journal>
    {

        protected new IJournalService Service => (IJournalService)base.Service;
        public JournalsController()
            : base(Factory.CreateService<JournalService>())
        {
        }

        protected override string FieldsForList => FieldListBuilder
            .Include(p => p.JournalNumber)
            .Include(p => p.Id)
            .Include(p => p.TransactionDate)
            .Include(p => p.JournalType);

        protected override string FieldsForAll => FieldListBuilder
            .IncludeAll()
            .Exclude(p => p.FiscalYear)
            .Exclude(p => p.ParentJournal)
            .Exclude(p => p.ChildJournals)
            .Exclude(p => p.BusinessUnit)
            .Exclude(p => p.JournalLines.First().Journal)
            .Exclude(p => p.JournalLines.First().LedgerAccount)
            .Exclude(p => p.JournalLines.First().SourceBusinessUnit)
            .Exclude(p => p.JournalLines.First().SourceFund);
            
        protected override Expression<Func<Journal, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Journal, object>>[]
            {
                c => c.JournalLines, c => c.ParentJournal
            };
        }

        protected override string FieldsForSingle => new PathHelper.FieldListBuilder<Journal>().Exclude(p => p.BusinessUnit)
                                                                                               .Exclude(p => p.FiscalYear)
                                                                                               .Exclude(p => p.JournalLines.First().Journal)
                                                                                               .Exclude(p => p.ChildJournals)
                                                                                               .Exclude(p => p.ParentJournal.BusinessUnit)
                                                                                               .Exclude(p => p.ParentJournal.FiscalYear)
                                                                                               .Exclude(p => p.ParentJournal.JournalLines)
                                                                                               .Exclude(p => p.ParentJournal.ChildJournals)
                                                                                               .Exclude(p => p.ParentJournal.ParentJournal);

        [HttpGet]
        [Route("api/v1/journals/{id}", Name = RouteNames.Journal + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/journals", Name = RouteNames.Journal)]
        public IHttpActionResult GetJournals(Guid? businessUnitId = null,
                                             Guid? fiscalYearId = null,
                                             string journalType = null,
                                             int journalNumber = 0,
                                             string queryString = null,
                                             string comment = null,
                                             string lineItemComment = null,
                                             decimal amountFrom = 0m,
                                             decimal amountTo = 0m,
                                             DateTime? transactionDateFrom = null,
                                             DateTime? transactionDateTo = null,
                                             string createdBy = null,
                                             DateTime? createdOnFrom = null,
                                             DateTime? createdOnTo = null,
                                             string journalStatus = null,
                                             string fields = null,
                                             int? offset = SearchParameters.OffsetDefault,
                                             int? limit = SearchParameters.LimitDefault,
                                             string orderBy = OrderByProperties.EntityNumber
            )
        {
            var search = new JournalSearch()
            {
                BusinessUnitId = businessUnitId,
                FiscalYearId = fiscalYearId,
                JournalNumber = journalNumber,
                QueryString = queryString,
                Comment = comment,
                LineItemComment = lineItemComment,
                AmountFrom = amountFrom,
                AmountTo = amountTo,
                TransactionDateFrom = transactionDateFrom,
                TransactionDateTo = transactionDateTo,
                CreatedBy = createdBy,
                CreatedOnFrom = createdOnFrom,
                CreatedOnTo = createdOnTo,
                JournalStatus = journalStatus,
                Offset = offset,
                Limit = limit,
                OrderBy = orderBy,
            };

            if (!string.IsNullOrWhiteSpace(journalType))
            {
                search.JournalType = EnumHelper.ConvertToEnum<JournalType>(journalType);
            }

            return base.GetAll(RouteNames.Journal, search, fields);
        }

        [HttpGet]
        [Route("api/v1/journals/new/{journaltype}")]
        public IHttpActionResult NewJournal(string journaltype, Guid? businessUnitId = null, Guid? fiscalYearId = null, string fields=null)
        {
            try
            {
                JournalType type = EnumHelper.ConvertToEnum<JournalType>(journaltype);

                var journal = Service.NewJournal(type, businessUnitId, fiscalYearId);
                fields = ConvertFieldList(fields, FieldsForSingle);
                return FinalizeResponse(journal, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("api/v1/journals", Name = RouteNames.Journal + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Journal item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/journals/{id}", Name = RouteNames.Journal + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/journals/{id}", Name = RouteNames.Journal + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}