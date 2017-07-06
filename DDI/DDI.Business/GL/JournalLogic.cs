using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Business.Core;
using DDI.Business.Helpers;
using DDI.Search;
using DDI.Search.Models;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
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
                    if (approvals.All(p => p.ApprovedById != null))
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

            if (entity.IsReversed)
            {
                statusTerms.Add(EntityStatus.Reversed);
            }

            if (entity.DeletionDate != null)
            {
                statusTerms.Add(EntityStatus.Deleted);
            }

            document.Status = string.Join(" ", statusTerms);
            return document;
        }

        /// <summary>
        /// Format the journal status as a string.
        /// </summary>
        /// <param name="journal">Journal object</param>
        /// <returns></returns>
        public string GetJournalStatusDescription(Journal journal)
        {
            string journalTypeName = LinkedEntityHelper.GetEntityTypeName<Journal>();

            if (journal.DeletionDate.HasValue)
            {
                return $"Deleted on {journal.DeletionDate:d}";
            }

            if (journal.JournalType == JournalType.Recurring)
            {
                if (journal.IsExpired)
                {
                    return "Expired";
                }
            }

            if (journal.JournalType == JournalType.Normal)
            {
                string result = string.Empty;

                var approvals = UnitOfWork.Where<EntityApproval>(p => p.EntityType == journalTypeName && p.ParentEntityId == journal.Id);
                if (approvals.Count() > 0)
                {
                    if (approvals.All(p => p.ApprovedById != null))
                    {
                        result = "Approved";
                        var approval = approvals.First();
                        var approver = UnitOfWork.GetReference(approval, p => p.ApprovedBy);
                        if (approver != null)
                        {
                            result += $" by {approver.FullName}";
                        }
                        if (approval.ApprovedOn.HasValue)
                        {
                            result += $" on {approval.ApprovedOn.ToRoundTripString()}";
                        }
                    }
                    else
                    {
                        return "Unapproved";
                    }
                }

                var transactions = UnitOfWork.GetEntities<EntityTransaction>(p => p.Transaction)
                                             .Where(p => p.EntityType == journalTypeName && p.ParentEntityId == journal.Id)
                                             .Select(p => p.Transaction);

                if (result.Length > 0)
                {
                    result += ", ";
                }
                if (transactions.All(p => p.PostDate != null))
                {
                    result += $"Posted on {transactions.Max(p => p.PostDate.Value).ToRoundTripString()}";
                }
                else
                {
                    result += "Unposted";
                }
                return result;
            }

            return "Active";
        }

        /// <summary>
        /// Get a description for the journal's parent journal
        /// </summary>
        public string GetJournalParentDescription(Journal journal)
        {
            if (journal == null)
            {
                throw new ArgumentNullException(nameof(journal));
            }
            if (journal.ParentJournalId == null)
            {
                return string.Empty;
            }

            Journal parent = UnitOfWork.GetReference(journal, p => p.ParentJournal);
            if (parent == null)
            {
                return string.Empty;
            }

            return "Copied from " + GetJournalDescription(parent);
        }

        /// <summary>
        /// Get a description for a journal.
        /// </summary>
        public string GetJournalDescription(Journal journal)
        {
            if (journal == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            switch (journal.JournalType)
            {
                case JournalType.Normal:
                    sb.Append("One-Time Journal");
                    break;
                case JournalType.Recurring:
                    sb.Append("Recurring Journal");
                    break;
                case JournalType.Template:
                    sb.Append("Journal Template");
                    break;
            }

            sb.Append(" #").Append(journal.JournalNumber);

            if (journal.JournalType == JournalType.Normal)
            {
                var fiscalyear = UnitOfWork.GetReference(journal, p => p.FiscalYear);
                sb.Append(" for ").Append(fiscalyear.Name);
            }

            return sb.ToString();
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
