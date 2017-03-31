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
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.GL
{
    public class AccountLogic : EntityLogicBase<Account>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AccountLogic));
        public AccountLogic() : this(new UnitOfWorkEF()) { }

        public AccountLogic(IUnitOfWork unitOfWork) : base(unitOfWork)
        {            
        }

        public override void Validate(Account entity)
        {
            base.Validate(entity);

        }

        /// <summary>
        /// Calcualte an account number for an Account.
        /// </summary>
        public string CalculateAccountNumber(Account account)
        {
            if (account == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            var ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();

            // Ensure account's referenced entities are fully populated.
            if (account.AccountSegments == null || account.FiscalYear == null || account.AccountSegments.FirstOrDefault()?.Segment == null)
            {
                account = UnitOfWork.GetEntities<Account>(p => p.AccountSegments.First().Segment, p => p.FiscalYear).FirstOrDefault(p => p.Id == account.Id);
            }

            var segmentLevels = ledgerLogic.GetSegmentLevels(account.FiscalYear.LedgerId.Value);

            foreach (var segment in account.AccountSegments.OrderBy(p => p.Level).Select(p => p.Segment))
            {
                if (sb.Length > 0 && !string.IsNullOrWhiteSpace(segment.Code))
                {
                    sb.Append(segmentLevels[segment.Level - 2].Separator);                    
                }
                sb.Append(segment.Code);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calcualte an account number for a list of Segment entities.
        /// </summary>
        public string CalculateAccountNumber(Ledger ledger, IList<Segment> segments)
        {
            if (segments == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            var ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();


            var segmentLevels = ledgerLogic.GetSegmentLevels(ledger);

            int thisLevel = 0;
            foreach (var segment in segments.OrderBy(p => p.Level))
            {
                // Create placeholders (e.g. ???) for missing segments.  (Trailing blank segments have already been removed from the enumeration.)
                while (++thisLevel < segment.Level)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(segmentLevels[thisLevel - 2].Separator);
                    }
                    sb.Append(new string('?', segmentLevels[thisLevel].Length));
                }

                if (sb.Length > 0 && !string.IsNullOrWhiteSpace(segment.Code))
                {
                    sb.Append(segmentLevels[segment.Level - 2].Separator);
                }
                sb.Append(segment.Code);
            }

            return sb.ToString();
        }

    }
}
