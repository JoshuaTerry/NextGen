﻿using DDI.Shared.Models.Client.GL;
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

namespace DDI.Services.GL
{
    public class PostedTransactionService : ServiceBase<PostedTransaction>
    {
        public IEnumerable<PostedTransaction> GetPostedTransactionsForLedgerAccountYearId(Guid ledgerAccountId)
        {
            return UnitOfWork.GetEntities<PostedTransaction>().Where(pt=> pt.LedgerAccountYearId == ledgerAccountId) ; ;
        }
    }
}