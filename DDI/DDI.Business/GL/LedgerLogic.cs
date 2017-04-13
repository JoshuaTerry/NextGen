using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class LedgerLogic : EntityLogicBase<Ledger>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(LedgerLogic));
        public LedgerLogic() : this(new UnitOfWorkEF()) { }
        private IRepository<Ledger> _ledgerRepository;
        private IRepository<SegmentLevel> _segmentLevelRepository;

        public LedgerLogic(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _ledgerRepository = unitOfWork.GetRepository<Ledger>();
            _segmentLevelRepository = unitOfWork.GetRepository<SegmentLevel>();
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
            set
            {
                _ledgerCache = value;
            }
        }

        public override void Validate(Ledger ledger)
        {
            if (ledger == null)
            {
                throw new ArgumentNullException(nameof(ledger));
            }

            ValidateNonBlankAndUnique(3, "Budget names", ledger.FixedBudgetName, ledger.WorkingBudgetName, ledger.WhatIfBudgetName);

            if (ledger.AccountGroupLevels < 0 || ledger.AccountGroupLevels > ConstantsGL.MaxAccountGroups)
            {
                throw new ValidationException(UserMessagesGL.AccountGroupLevelsRange);
            }

            if (ledger.NumberOfSegments < 0 || ledger.NumberOfSegments > ConstantsGL.MaxAccountSegments)
            {
                throw new ValidationException(UserMessagesGL.AccountSegmentsRange);
            }

            ValidateNonBlankAndUnique(ledger.AccountGroupLevels, "Account group labels", ledger.AccountGroup1Title, ledger.AccountGroup2Title, ledger.AccountGroup3Title, ledger.AccountGroup4Title);

            BusinessUnit unit = UnitOfWork.GetReference(ledger, p => p.BusinessUnit);

            // Additional validation based on modified properties.

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

                if (modifiedProperties.Contains(nameof(ledger.AccountGroupLevels)))
                {
                    // # of account group levels changing - ensure no G/L accounts exist.
                    if (UnitOfWork.Any<Account>(p => p.LedgerAccountYears.Any(q => q.FiscalYear.LedgerId == ledger.Id)))
                    {
                        throw new ValidationException(UserMessagesGL.AccountGroupLevelsChanged);
                    }
                }

                if (modifiedProperties.Contains(nameof(ledger.SegmentLevels)))
                {
                    // # of segment levels changing - ensure no segments exist.
                    if (UnitOfWork.Any<Segment>(p => p.FiscalYear.LedgerId == ledger.Id))
                    {
                        throw new ValidationException(UserMessagesGL.SegmentLevelsChanged);
                    }
                }

                // If modifying a ledger for an organizational business unit, the settings should be copied to all the other common business units.
                if (ledger.IsParent)
                {
                    CopyLedgerProperties(ledger, ledger.Id);
                }

                else if (unit.BusinessUnitType == BusinessUnitType.Common && ledger.OrgLedgerId != null)
                {
                    CopyLedgerProperties(ledger, ledger.OrgLedgerId.Value);
                }
            }

            ValidateSegmentLevels(ledger);

            if (ledger.IsParent)
            {
                CopySegmentLevels(ledger, ledger.Id);
            }

            else if (unit.BusinessUnitType == BusinessUnitType.Common && ledger.OrgLedgerId != null)
            {
                CopySegmentLevels(ledger, ledger.OrgLedgerId.Value);
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
            //return LedgerCache.Entities.FirstOrDefault(p => p.BusinessUnitId == businessUnitId && p.FiscalYears.Any(q => q.StartDate <= dt && q.EndDate >= dt));

            foreach (var ledger in LedgerCache.Entities)
            {
                if (ledger.BusinessUnitId == businessUnitId && ledger.FiscalYears.Any(q => q.StartDate <= dt && q.EndDate >= dt))
                {
                    return ledger;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the ledger that contains a valid fiscal year for the current business date, or if no year defined, the latest year.
        /// </summary>
        public Ledger GetCurrentLedger(Guid businessUnitId)
        {
            return GetCurrentLedger(businessUnitId, DateTime.Now.Date)
                   ??
                   GetMostRecentLedger(businessUnitId);
        }

        internal Ledger GetMostRecentLedger(Guid businessUnitId)
        {
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
        private void CopyLedgerProperties(Ledger from, Guid orgLedgerId)
        {
            foreach (Ledger to in UnitOfWork.Where<Ledger>(p => p.Id != from.Id &&
                                 (p.OrgLedgerId == orgLedgerId || p.Id == orgLedgerId)))
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

        private LedgerStatus GetCommonLedgerStatus(Ledger ledger)
        {
            if (ledger == null)
            {
                throw new ArgumentNullException(nameof(ledger));
            }

            BusinessUnit unit = UnitOfWork.GetReference(ledger, p => p.BusinessUnit);
            if (unit.BusinessUnitType == BusinessUnitType.Separate)
            {
                return ledger.Status;
            }

            bool allEmpty = true;
            bool allClosed = true;

            foreach (var entry in UnitOfWork.Where<Ledger>(p => p.BusinessUnit.BusinessUnitType != BusinessUnitType.Separate))
            {
                allEmpty = allEmpty && entry.Status == LedgerStatus.Empty;
                allClosed = allClosed && entry.Status == LedgerStatus.Closed;
            }

            if (allEmpty)
            {
                return LedgerStatus.Empty;
            }
            else if (allClosed)
            {
                return LedgerStatus.Closed;
            }

            return LedgerStatus.Active;

        }

        private void CopySegmentLevels(Ledger ledger)
        {

        }

        private void ValidateSegmentLevels(Ledger ledger)
        {
            // Segment levels can be modified only if this ledger has a status of empty and if common, all other common ledgers are empty.
            bool isEmpty = GetCommonLedgerStatus(ledger) == LedgerStatus.Empty;

            if (ledger.SegmentLevels != null && !isEmpty)
            {
                string[] propertiesToCheck = new string[] { nameof(SegmentLevel.Format), nameof(SegmentLevel.IsCommon), nameof(SegmentLevel.IsLinked),
                    nameof(SegmentLevel.Length), nameof(SegmentLevel.Separator), nameof(SegmentLevel.Type) };

                foreach (var level in ledger.SegmentLevels)
                {
                    bool isModified = false;
                    List<string> modifiedProperties = null;
                    if (_segmentLevelRepository.GetEntityState(level) != EntityState.Added)
                    {
                        modifiedProperties = _segmentLevelRepository.GetModifiedProperties(level);
                        isModified = (modifiedProperties.Count > 0);
                    }
                    else
                    {
                        isModified = true;
                    }

                    if (isModified && modifiedProperties.Intersect(propertiesToCheck).Count() > 0)
                    {
                        throw new ValidationException(UserMessagesGL.SegmentLevelsNotEditable);
                    }
                }
            }

            if (ledger.SegmentLevels != null)
            {
                // Validate each segment level
                for (int level = 1; level <= ledger.NumberOfSegments; level++)
                {
                    var segments = ledger.SegmentLevels.Where(p => p.Level == level);
                    if (segments.Count() == 0)
                    {
                        throw new ValidationException(UserMessagesGL.SegmentLevelMissing, level.ToString());
                    }

                    if (segments.Count() > 1)
                    {
                        throw new ValidationException(UserMessagesGL.SegmentLevelDuplicate, level.ToString());
                    }

                    SegmentLevel segment = segments.First();

                    if (segment.IsLinked && segment.Level == 1)
                    {
                        throw new ValidationException(UserMessagesGL.SegmentLevelOneLinked);
                    }

                }

                // Validate segment level type.

                int fundSegments = ledger.SegmentLevels.Count(p => p.Type == SegmentType.Fund);

                if (ledger.FundAccounting && fundSegments != 1)
                {
                    throw new ValidationException(UserMessagesGL.SegmentLevelFundOne);
                }
                else if (!ledger.FundAccounting && fundSegments != 0)
                {
                    throw new ValidationException(UserMessagesGL.SegmentLevelFundZero);
                }

                // Validate level abbreviation and name

                ValidateNonBlankAndUnique(0, "Segment level abbreviation", ledger.SegmentLevels.Select(p => p.Abbreviation).ToArray());
                ValidateNonBlankAndUnique(0, "Segment level label", ledger.SegmentLevels.Select(p => p.Name).ToArray());
                
            }
        }

        private void CopySegmentLevels(Ledger from, Guid orgLedgerId)
        {
            if (from.SegmentLevels == null)
            {
                return;
            }

            foreach (Ledger to in UnitOfWork.GetEntities<Ledger>(p => p.SegmentLevels)
                                            .Where(p => p.Id != from.Id && 
                                                (p.OrgLedgerId == orgLedgerId || p.Id == orgLedgerId)))
            {
                // add/update levels 
                foreach (var fromLevel in from.SegmentLevels)
                {
                    SegmentLevel toLevel = to.SegmentLevels.FirstOrDefault(p => p.Level == fromLevel.Level);
                    if (toLevel == null)
                    {
                        toLevel = new SegmentLevel()
                        {
                            Level = fromLevel.Level
                        };
                        to.SegmentLevels.Add(toLevel);
                    }
                    toLevel.Abbreviation = fromLevel.Abbreviation;
                    toLevel.Format = fromLevel.Format;
                    toLevel.IsCommon = fromLevel.IsCommon;
                    toLevel.IsLinked = fromLevel.IsLinked;
                    toLevel.Length = fromLevel.Length;
                    toLevel.Name = fromLevel.Name;
                    toLevel.Separator = fromLevel.Separator;
                    toLevel.SortOrder = fromLevel.SortOrder;
                    toLevel.Type = fromLevel.Type;                    
                }

                // Remove levels
                foreach (var toLevel in to.SegmentLevels.ToList())
                {
                    if (!from.SegmentLevels.Any(p => p.Level == toLevel.Level))
                    {
                        to.SegmentLevels.Remove(toLevel);
                    }
                }
            }
        }
    }
}
