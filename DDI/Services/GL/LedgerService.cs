using DDI.Shared.Models.Client.GL;
using DDI.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using DDI.Business.GL;

namespace DDI.Services.GL
{
    public class LedgerService : ServiceBase<Ledger>
    {
        private LedgerLogic _ledgerLogic = null;

        public LedgerService(IUnitOfWork uow, LedgerLogic logic) : base(uow)
        {
            _ledgerLogic = logic;
        }

        protected override Action<Ledger, string> FormatEntityForGet => FormatLedgerForGet;

        private void FormatLedgerForGet(Ledger ledger, string fields)
        {
            ledger.DisplayFormat = _ledgerLogic.GetGLAccountFormatSample(ledger);
            ledger.HasLedgerAccounts = UnitOfWork.Any<LedgerAccount>(p => p.LedgerId == ledger.Id);
        }
    }
}
