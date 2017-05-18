using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Data.Helpers;
using DDI.Search;
using DDI.Search.Models;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class JournalLogic : EntityLogicBase<Journal>
    {
        public JournalLogic(IUnitOfWork uow) : base(uow)
        {
        }

        public override void Validate(Journal journal)
        {
            journal.AssignPrimaryKey();
            if (journal.JournalLines != null)
            {
                // Ensure journal lines are linked properly to the journal.
                foreach (var line in journal.JournalLines)
                {
                    line.Journal = journal;
                    line.JournalId = journal.Id;
                }
            }

            ScheduleUpdateSearchDocument(journal);
        }

        public override void UpdateSearchDocument(Journal journal)
        {
            var elasticRepository = new ElasticRepository<JournalDocument>();
            elasticRepository.Update((JournalDocument)BuildSearchDocument(journal));
        }

        public override ISearchDocument BuildSearchDocument(Journal entity)
        {
            string journalTypeName = LinkedEntityHelper.GetEntityTypeName<Journal>();

            var document = new JournalDocument()
            {
                Id = entity.Id,
                JournalNumber = entity.JournalNumber,
                Amount = entity.Amount,
                FiscalYearId = entity.FiscalYearId,
                BusinessUnitId = entity.BusinessUnitId,
                Comment = entity.Comment,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                TransactionDate = entity.TransactionDate,
                JournalType = (int)entity.JournalType
            };

            if (entity.JournalLines != null)
            {
                document.LineItemComments = string.Join(" ", entity.JournalLines.Select(p => p.Comment).Where(p => !string.IsNullOrWhiteSpace(p)));
            }
            
            // Status
            List<string> statusTerms = new List<string>();
            if (entity.DeletionDate.HasValue)
            {
                statusTerms.Add(EntityStatus.Deleted);
            }

            if (entity.JournalType == JournalType.Recurring)
            {
                if (entity.IsExpired)
                {
                    statusTerms.Add(EntityStatus.Expired);
                }
                else
                {
                    statusTerms.Add(EntityStatus.Active);
                }
            }

            if (entity.JournalType == JournalType.Normal)
            {
                var approvals = UnitOfWork.Where<EntityApproval>(p => p.EntityType == journalTypeName && p.ParentEntityId == entity.Id);
                if (approvals.Count() > 0)
                {
                    if (approvals.All(p => p.AppprovedById != null))
                    {
                        statusTerms.Add(EntityStatus.Approved);
                    }
                    else
                    {
                        statusTerms.Add(EntityStatus.Unapproved);
                    }
                }

                var transactions = UnitOfWork.GetEntities<EntityTransaction>(p => p.Transaction)
                                             .Where(p => p.EntityType == journalTypeName && p.ParentEntityId == entity.Id)
                                             .Select(p => p.Transaction);

                if (transactions.All(p => p.PostDate != null))
                {
                    statusTerms.Add(EntityStatus.Posted);
                }
                else
                {
                    statusTerms.Add(EntityStatus.Unposted);
                }
            }

            document.Status = string.Join(" ", statusTerms);
            return document;
        }

        /// <summary>
        /// Create a new journal.
        /// </summary>
        /// <param name="journalType">Journal type</param>
        /// <param name="businessUnitId">Business unit ID (for recurring or template journals)</param>
        /// <param name="fiscalYearId">Fiscal Year ID (for normal journals)</param>
        public Journal NewJournal(JournalType journalType, Guid? businessUnitId, Guid? fiscalYearId)
        {
            Journal journal = new Journal() { JournalType = journalType };

            
            EntityNumberType numberType = EntityNumberType.Journal;
            Guid entityId;

            if (journalType == JournalType.Normal)
            {
                if (fiscalYearId.HasValue)
                {
                    journal.FiscalYear = UnitOfWork.GetById<FiscalYear>(fiscalYearId.Value, p => p.Ledger.BusinessUnit);
                }
                if (journal.FiscalYear == null)
                {
                    throw new ValidationException(UserMessagesGL.NewJournalNoFiscalYear);
                }
                journal.BusinessUnit = journal.FiscalYear.Ledger.BusinessUnit;
                journal.FiscalYearId = journal.FiscalYear.Id;
                entityId = journal.FiscalYearId.Value;
                numberType = EntityNumberType.Journal;
            }
            else
            {
                if (businessUnitId.HasValue)
                {
                    journal.BusinessUnit = UnitOfWork.GetById<BusinessUnit>(businessUnitId.Value);
                }
                if (journal.BusinessUnit == null)
                {
                    throw new ValidationException(UserMessagesGL.NewJournalNoBusinessUnit);
                }
                entityId = journal.BusinessUnit.Id;
                if (journalType == JournalType.Recurring)
                {
                    numberType = EntityNumberType.RecurringJournal;
                }
                else
                {
                    numberType = EntityNumberType.JournalTemplate;
                }

            }

            journal.BusinessUnitId = journal.BusinessUnit.Id;
            journal.JournalNumber = UnitOfWork.GetBusinessLogic<EntityNumberLogic>().GetNextEntityNumber(numberType, entityId);
            journal.JournalLines = new List<JournalLine>();
                       
            journal.AssignPrimaryKey();

            return journal;
        }

    }
}
