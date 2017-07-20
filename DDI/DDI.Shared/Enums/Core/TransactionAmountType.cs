﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{
    /// <summary>
    /// For an EntityTransaction, describes how the transaction amount relates to the entity.
    /// </summary>
    public enum TransactionAmountType
    {
        [Description("None")]
        None = 0,

        // Investments: 1 - 199

        [Description("Investment Balance")]
        InvestmentBalance = 1,

        [Description("Investment Interest")]
        InvestmentInterest = 2,

        // Loans: 200-499

        [Description("Loan Principal")]
        LoanPrincipal = 200,

        [Description("Loan Interest")]
        LoanInterest = 201,

        // Portfolio: 600 - 699

        // Line of Credit: 800 - 899

        // Accounts Payable: 10000 - 19999

        // Accounts Receivable: 20000 - 29999

        // Cash Receipts: 40000 - 49999

        [Description("Receipt Processed")]
        ReceiptProcessed = 40000,

        [Description("Receipt Reversed")]
        ReceiptReversed = 40001

        // Cash Disbursements: 30000 - 39999

        // Fixed Assets: 50000 - 59999

        // Inventory: 60000 - 69999

        // Journals:  70000 - 79999

        // Donations:  100000 - 109999

        // Campaigns/Appeals:  101000 - 101999

        // Planned Giving:  102000 - 102999

        // Named Funds:  103000 - 103999

        // Events/Programs:  104000 - 104999

        // Health Policy:  190000 - 199999

        // CRM:  200000 - 209999



    }
}
