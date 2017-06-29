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
        private string _journalTypeName;

        public JournalLogic(IUnitOfWork uow) : base(uow)
        {
            _journalTypeName = LinkedEntityHelper.GetEntityTypeName<Journal>();
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
                var approvals = UnitOfWork.Where<EntityApproval>(p => p.EntityType == _journalTypeName && p.ParentEntityId == entity.Id);
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
                                             .Where(p => p.EntityType == _journalTypeName && p.ParentEntityId == entity.Id)
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

        public IList<Transaction> CreateTransactions(IEntity entity, bool reverse = false, DateTime? transactionDate = null)
        {
            Journal journal = entity as Journal;
            if (journal == null)
            {
                throw new ArgumentException("No journal was provided.", nameof(entity));
            }

            Int64 transactionNumber = 0;
            int lineNumber = 0;
            var trans = new List<Transaction>();
            TransactionType transactionType = TransactionType.Journal;
            FiscalYearLogic yearLogic = UnitOfWork.GetBusinessLogic<FiscalYearLogic>();
            AccountLogic accountLogic = UnitOfWork.GetBusinessLogic<AccountLogic>();
            FundLogic fundLogic = UnitOfWork.GetBusinessLogic<FundLogic>();
            TransactionLogic transactionLogic = UnitOfWork.GetBusinessLogic<TransactionLogic>();

            bool isAdjustment = false;

            bool isUnapproved = UnitOfWork.Any<EntityApproval>(p => p.EntityType == _journalTypeName && p.ParentEntityId == journal.Id && p.ApprovedOn == null);
            bool isApproved = !isUnapproved &&
                              UnitOfWork.Any<EntityApproval>(p => p.EntityType == _journalTypeName && p.ParentEntityId == journal.Id && p.ApprovedOn != null);

            TransactionStatus transactionStatus = isUnapproved ? TransactionStatus.Pending : TransactionStatus.Unposted;

            if (transactionDate == null)
            {
                transactionDate = journal.TransactionDate;
            }

            if (transactionDate == null)
            {
                throw new InvalidOperationException(string.Format(UserMessages.TranDateMissingForEntity, GetJournalDescription(journal)));
            }

            // Ensure journal has a business unit.
            if (journal.BusinessUnitId == null)
            {
                throw new InvalidOperationException("Journal business unit id cannot be null.");
            }

            // Determine the fiscal year.
            FiscalYear year = yearLogic.GetFiscalYear(journal.BusinessUnitId.Value, transactionDate);
            if (year == null)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.NoFiscalYearForDate, transactionDate.ToShortDateString()));
            }

            FiscalPeriod period = yearLogic.GetFiscalPeriod(year, transactionDate.Value);
            
            // If the fiscal year is closed, this must be an adjustment.
            if (year.Status == FiscalYearStatus.Closed)
            {
                if (!period.IsAdjustmentPeriod)
                {
                    throw new InvalidOperationException(string.Format(UserMessagesGL.TranDateClosedPeriod, transactionDate.ToShortDateString()));
                }
                isAdjustment = true;
            }
            // Otherwise, verify the fiscal period is open.
            else if (period.Status == FiscalPeriodStatus.Closed)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.TranDateClosedPeriod, transactionDate.ToShortDateString()));
            }            

            // Get list of existing journal entity transactions
            var existingTrans = UnitOfWork.GetEntities<EntityTransaction>(p => p.Transaction)
                                          .Where(p => p.EntityType == _journalTypeName && p.ParentEntityId == journal.Id && p.Relationship == EntityTransactionRelationship.Owner)
                                          .ToList();

            bool isPosted = existingTrans.Any(p => p.Transaction.Status == TransactionStatus.Posted);
            
            if (reverse)
            {
                if (isUnapproved)
                {
                    throw new InvalidOperationException("Journal is unapproved and cannot be reversed.");
                }
                if (!isPosted)
                {
                    throw new InvalidOperationException("Journal is unposted and cannot be reversed.");
                }
                if (existingTrans.Any(p => p.Transaction.Status == TransactionStatus.Reversed))
                {
                    throw new InvalidOperationException(string.Format(UserMessages.EntityAlreadyReversed, GetJournalDescription(journal)));
                }
                transactionType = TransactionType.JournalReversal;
            }
            else if (isPosted)
            {
                throw new InvalidOperationException(string.Format(UserMessages.EntityAlreadyPosted, GetJournalDescription(journal)));
            }

            // Reuse the existing transaction number.
            if (existingTrans.Count > 0)
            {
                transactionNumber = existingTrans.Max(p => p.Transaction.TransactionNumber);

                // Ensure starting line number is > than any existing line number for non-pending transactions.
                var pendingTrans = existingTrans.Where(p => p.Transaction.TransactionNumber == transactionNumber && p.Transaction.Status != TransactionStatus.Pending);
                if (pendingTrans.Count() > 0)
                {
                    lineNumber = pendingTrans.Max(p => p.Transaction.LineNumber);
                }
            }

            // Create the list of transactions to be created.
            foreach (var line in UnitOfWork.GetReference(journal, p => p.JournalLines).Where(p => p.Amount != 0m && p.LedgerAccountId != null).OrderBy(p => p.LineNumber))
            {
                var tran = new Transaction()
                {
                    TransactionNumber = transactionNumber,
                    LineNumber = ++lineNumber,
                    Amount = line.Amount,
                    Description = StringHelper.FirstNonBlank(line.Comment, journal.Comment) ?? string.Empty,
                    FiscalYearId = journal.FiscalYearId,
                    IsAdjustment = isAdjustment,
                    Status = transactionStatus,
                    TransactionType = transactionType,
                    TransactionDate = transactionDate,
                };

                LedgerAccountYear account = accountLogic.GetLedgerAccountYear(line.LedgerAccountId, year);
                tran.DebitAccount = account;
                tran.DebitAccountId = account?.Id;
                tran.CreditAccountId = null;

                if (tran.DebitAccountId == null)
                {
                    LedgerAccount ledgerAccount = UnitOfWork.GetReference(line, p => p.LedgerAccount);
                    throw new InvalidOperationException(string.Format(UserMessagesGL.GLAccountNotInFiscalYear, ledgerAccount?.AccountNumber ?? "(Undefined)", year.Name));
                }

                if (reverse)
                {
                    tran.Amount = -tran.Amount;
                }

                tran.AssignPrimaryKey();
                trans.Add(tran);

                // Create fund accounting transactions...

                // First, get the fund and the fiscal year for the line item G/L account.
                Fund fund = year.Ledger.FundAccounting ? fund = fundLogic.GetFund(UnitOfWork.GetReference(account, p => p.Account)) : null;
                FiscalYear accountYear = yearLogic.GetCachedFiscalYear(account.FiscalYearId);
                Guid? sourceFundId = line.SourceFundId;
                bool interUnit = false;

                if (line.SourceBusinessUnitId != null && line.SourceBusinessUnitId != accountYear.Ledger.BusinessUnitId)
                {
                    // Business units are different.  (Inter-unit transfer required.)

                    interUnit = true;

                    // Get a BusinessUnitFromTo                    
                    LedgerAccountYear offsettingAccount = null;
                    BusinessUnitFromTo fromTo = fundLogic.GetBusinessUnitFromTo(accountYear.Id, line.SourceBusinessUnitId.Value);
                    if (fromTo != null)
                    {
                        Guid? offsettingLedgerAccountId = (line.DueToMode == DueToMode.DueFrom ? fromTo.FromLedgerAccountId : fromTo.ToLedgerAccountId);
                        offsettingAccount = accountLogic.GetLedgerAccountYear(offsettingLedgerAccountId, fromTo.FiscalYearId);
                    }

                    if (offsettingAccount == null)
                    {
                        BusinessUnit sourceUnit = UnitOfWork.GetById<BusinessUnit>(line.SourceBusinessUnitId.Value);
                        throw new InvalidOperationException(string.Format(UserMessagesGL.NoUnitFromTo, accountYear.Ledger.BusinessUnit.Code, sourceUnit.Code));
                    }
                    
                    if (year.Ledger.FundAccounting)
                    {
                        // Source fund is now the offsetting account's fund.  (This fund will be in the same business unit as the transaction.)
                        sourceFundId = fundLogic.GetFund(UnitOfWork.GetReference(offsettingAccount, p => p.Account)).Id;
                    }

                    // Create a transaction for the offsetting business unit.
                    Transaction tran2 = transactionLogic.Duplicate(tran);
                    tran2.LineNumber = ++lineNumber;
                    tran2.Amount = -tran.Amount;
                    tran2.DebitAccount = offsettingAccount;
                    tran2.DebitAccountId = offsettingAccount.Id;
                    trans.Add(tran2);
                }

                if (fund != null && sourceFundId != null && fund.Id != sourceFundId)
                {
                    // Funds are different.  (Inter-fund transfer within the same business unit.)
                    // Get a FundFromTo.
                    LedgerAccountYear offsettingAccount = null;
                    FundFromTo fromTo = fundLogic.GetFundFromTo(fund.Id, sourceFundId.Value);
                    if (fromTo != null)
                    {
                        Guid? offsettingLedgerAccountId = (line.DueToMode == DueToMode.DueFrom ? fromTo.FromLedgerAccountId : fromTo.ToLedgerAccountId);
                        offsettingAccount = accountLogic.GetLedgerAccountYear(offsettingLedgerAccountId, fromTo.FiscalYearId);
                    }

                    if (offsettingAccount == null)
                    {
                        string fund1 = UnitOfWork.GetById<Segment>(fund.FundSegmentId.Value)?.Code ?? string.Empty;
                        string fund2 = UnitOfWork.GetById<Fund>(line.SourceFundId.Value, p => p.FundSegment)?.FundSegment?.Code ?? string.Empty;
                        throw new InvalidOperationException(string.Format(UserMessagesGL.NoFundFromTo, fund1, fund2));
                    }

                    // Create a transaction for the offsetting fund.
                    Transaction tran2 = transactionLogic.Duplicate(tran);
                    tran2.LineNumber = ++lineNumber;
                    tran2.Amount = -tran.Amount;
                    tran2.DebitAccount = offsettingAccount;
                    tran2.DebitAccountId = offsettingAccount.Id;
                    trans.Add(tran2);

                    if (interUnit)
                    {
                        // A third transfer is required,  which is basically the reverse of the transfer just created above.
                        // This is because the inter-unit transfer resulted in an inter-fund transfer.
                        fromTo = fundLogic.GetFundFromTo(sourceFundId.Value, fund.Id);
                        if (fromTo != null)
                        {
                            Guid? offsettingLedgerAccountId = (line.DueToMode != DueToMode.DueFrom ? fromTo.FromLedgerAccountId : fromTo.ToLedgerAccountId);
                            offsettingAccount = accountLogic.GetLedgerAccountYear(offsettingLedgerAccountId, fromTo.FiscalYearId);
                        }

                        if (offsettingAccount == null)
                        {
                            string fund1 = UnitOfWork.GetById<Segment>(fund.FundSegmentId.Value)?.Code ?? string.Empty;
                            string fund2 = UnitOfWork.GetById<Fund>(line.SourceFundId.Value, p => p.FundSegment)?.FundSegment?.Code ?? string.Empty;
                            throw new InvalidOperationException(string.Format(UserMessagesGL.NoFundFromTo, fund1, fund2));
                        }

                        // Create a transaction for the offsetting fund.
                        tran2 = transactionLogic.Duplicate(tran);
                        tran2.LineNumber = ++lineNumber;
                        tran2.Amount = tran.Amount;
                        tran2.DebitAccount = offsettingAccount;
                        tran2.DebitAccountId = offsettingAccount.Id;
                        trans.Add(tran2);
                    }
                }
            } // Each journal line.
            

            return trans;
        }

    }
}
