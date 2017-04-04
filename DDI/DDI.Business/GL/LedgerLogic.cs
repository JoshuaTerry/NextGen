using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.GL
{
    public class LedgerLogic : EntityLogicBase<Ledger>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(LedgerLogic));
        public LedgerLogic() : this(new UnitOfWorkEF()) { }
        private IRepository<Ledger> _ledgerRepository;
        
        public LedgerLogic(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _ledgerRepository = unitOfWork.GetRepository<Ledger>();
        }

        private CachedRepository<Ledger> _ledgerCache = null;
        internal CachedRepository<Ledger> LedgerCache
        {
            get
            {
                if (_ledgerCache == null)
                {
                    _ledgerCache = new CachedRepository<Ledger>(UnitOfWork.GetRepository<Ledger>(),
                        p => p.SegmentLevels,
                        p => p.FiscalYears.First().FiscalPeriods,
                        p => p.OrgLedger,
                        p => p.DefaultFiscalYear,
                        p => p.BusinessUnit
                    );
                }
                return _ledgerCache;
            }
        }


        public override void Validate(Ledger ledger)
        {
            bool isModified = false;
            List<string> modifiedProperties = null;
            if (_ledgerRepository.GetEntityState(ledger) != EntityState.Added)
            {
                modifiedProperties = _ledgerRepository.GetModifiedProperties(ledger);
                isModified = (modifiedProperties.Count > 0);
            }
            else
            {
                isModified = true;
            }

            if (isModified)
            {
                if (_ledgerCache != null)
                {
                    _ledgerCache.InvalidateCache();
                }

                // If modifying a ledger for an organizational business unit, the settings should be copied to all the other common business units.
                if (ledger != null && UnitOfWork.GetReference(ledger, p => p.BusinessUnit).BusinessUnitType == BusinessUnitType.Organization)
                {
                    foreach (Ledger child in UnitOfWork.Where<Ledger>(p => p.OrgLedgerId == ledger.Id && p.BusinessUnit.BusinessUnitType != BusinessUnitType.Organization))
                    {
                        CopyProperties(ledger, child);
                    }
                }
            }            
        }
        
        public Ledger GetCachedLedger(Guid? ledgerId)
        {
            if (ledgerId == null)
            {
                return null;
            }

            return LedgerCache.GetById(ledgerId.Value);
        }

        /// <summary>
        /// Get the ledger that contains a valid fiscal year for a specified date.
        /// </summary>
        public Ledger GetCurrentLedger(Guid businessUnitId, DateTime dt)
        {
            return LedgerCache.Entities.FirstOrDefault(p => p.BusinessUnitId == businessUnitId && p.FiscalYears.Any(q => q.StartDate <= dt && q.EndDate >= dt));
        }

        /// <summary>
        /// Get the ledger that contains a valid fiscal year for the current business date, or if no year defined, the latest year.
        /// </summary>
        public Ledger GetCurrentLedger(Guid businessUnitId)
        {
            Ledger ledger = GetCurrentLedger(businessUnitId, DateTime.Now.Date);
            if (ledger != null)
                return ledger;
            IEnumerable<Ledger> unitLedgers = LedgerCache.Entities.Where(p => p.BusinessUnitId == businessUnitId);
            IEnumerable<FiscalYear> years = unitLedgers.Select(p => p.FiscalYears.OrderByDescending(q => q.StartDate)
                                                                             .FirstOrDefault()); // Get the latest fiscal year for each ledger.
            // If there aren't multiple years, return any ledger.
            if (years.Count() <= 1)
                return unitLedgers.FirstOrDefault();

            // Otherwise:
            return years.OrderByDescending(p => p.StartDate) // Order selected fiscal years in descending order
                        .FirstOrDefault()                  // Get latest overall
                        ?.Ledger;                          // Get the ledger
        }

        public SegmentLevel[] GetSegmentLevels(Ledger ledger)
        {
            if (ledger == null)
            {
                throw new ArgumentNullException(nameof(ledger));
            }
            if (ledger.SegmentLevels == null)
            {
                UnitOfWork.LoadReference(ledger, p => p.SegmentLevels);
            }
            return ledger.SegmentLevels.OrderBy(p => p.Level).ToArray();
        }

        public SegmentLevel[] GetSegmentLevels(Guid ledgerId)
        {
            return GetSegmentLevels(GetCachedLedger(ledgerId));
        }

        /// <summary>
        /// Get the segment level (1 - n) of the fund segment.  If fund accounting is not enabled, returns zero.
        /// </summary>
        public int GetFundSegmentLevel(Ledger ledger)
        {
            if (ledger.FundAccounting)
            {
                return ledger.SegmentLevels?.FirstOrDefault(p => p.Type == SegmentType.Fund)?.Level ?? 0;
            }
            return 0;
        }

        /// <summary>
        /// Get the fund segment's label from SegmentLevel.Name.
        /// </summary>
        public string GetFundLabel(Ledger ledger)
        {
            if (ledger.FundAccounting)
            {
                return ledger.SegmentLevels?.FirstOrDefault(p => p.Type == SegmentType.Fund)?.Name ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the client-specific name of a budget type.
        /// </summary>
        public string GetBudgetName(Ledger ledger, BudgetType budgetType)
        {
            switch (budgetType)
            {
                case BudgetType.Fixed: return StringHelper.FirstNonBlank(ledger.FixedBudgetName, "Fixed Budget");
                case BudgetType.Working: return StringHelper.FirstNonBlank(ledger.WorkingBudgetName, "Working Budget");
                case BudgetType.WhatIf: return StringHelper.FirstNonBlank(ledger.WhatIfBudgetName, "What-If Budget");
            }
            return string.Empty;
        }

        /// <summary>
        /// Copy properties from one ledger to another.
        /// </summary>
        private void CopyProperties(Ledger from, Ledger to)
        {
            to.AccountGroup1Title = from.AccountGroup1Title;
            to.AccountGroup2Title = from.AccountGroup2Title;
            to.AccountGroup3Title = from.AccountGroup3Title;
            to.AccountGroup4Title = from.AccountGroup4Title;
            to.AccountGroupLevels = from.AccountGroupLevels;
            to.ApproveJournals = from.ApproveJournals;
            to.CapitalizeHeaders = from.CapitalizeHeaders;
            to.CopyCOAChanges = from.CopyCOAChanges;
            to.FixedBudgetName = from.FixedBudgetName;
            to.FundAccounting = from.FundAccounting;
            to.NumberOfSegments = from.NumberOfSegments;
            to.PostAutomatically = from.PostAutomatically;
            to.PriorPeriodPostingMode = from.PriorPeriodPostingMode;
            to.Status = from.Status;
            to.WhatIfBudgetName = from.WhatIfBudgetName;
            to.WorkingBudgetName = from.WorkingBudgetName;
        }


    }
}
