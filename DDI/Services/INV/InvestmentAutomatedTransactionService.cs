using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.INV;


namespace DDI.Services
{
    public class InvestmentAutomatedTransactionService : ServiceBase<InvestmentAutomatedTransaction>
    {
        #region Public Methods

        
        public IDataResponse<InvestmentAutomatedTransaction> GetAutomatedTransactionById(Guid Id)
        {
            InvestmentAutomatedTransaction at = new InvestmentAutomatedTransaction();
            at.Id = new Guid();
            at.InvestmentAutomatedTransactionMethod = InvestmentAutomatedTransactionMethod.Deposit;
            at.NextTransactionDate = new DateTime(2017, 08, 15);
            at.RecurringType = Shared.Enums.Core.RecurringType.Monthly;
            at.Amount = new decimal(550);
            at.IsActive = true;
            at.PaymentMethodId = new Guid("e695bd55-fa89-4659-9b14-1a91b22fd603");



            var response = new DataResponse<InvestmentAutomatedTransaction>()
            {
                Data = at,
                IsSuccessful = true
            };

            return response;
        }




        public IDataResponse<List<InvestmentAutomatedTransaction>> GetAutomatedTransactionByInvestmentId(Guid Id)
        {


            InvestmentAutomatedTransaction[] at;
            at = new InvestmentAutomatedTransaction[3];

            at[0] = new InvestmentAutomatedTransaction();
            at[0].Id = new Guid();
            at[0].InvestmentAutomatedTransactionMethod = InvestmentAutomatedTransactionMethod.Deposit;
            at[0].NextTransactionDate = new DateTime(2017, 08, 15);
            at[0].RecurringType = Shared.Enums.Core.RecurringType.Monthly;
            at[0].Amount = new decimal(550);
            at[0].IsActive = true;
            at[0].PaymentMethod = new PaymentMethod();
            at[0].PaymentMethodId = at[0].PaymentMethod.Id;
            at[0].PaymentMethod.Description = "Checking";

            at[1] = new InvestmentAutomatedTransaction();
            at[1].Id = new Guid();
            at[1].InvestmentAutomatedTransactionMethod = InvestmentAutomatedTransactionMethod.Transfer;
            at[1].NextTransactionDate = new DateTime(2017, 08, 15);
            at[1].RecurringType = Shared.Enums.Core.RecurringType.Weekly;
            at[1].Amount = new decimal(550);
            at[1].IsActive = true;
            at[1].PaymentMethod = new PaymentMethod();
            at[1].PaymentMethodId = at[1].PaymentMethod.Id;
            at[1].PaymentMethod.Description = "Account";

            at[2] = new InvestmentAutomatedTransaction();
            at[2].Id = new Guid();
            at[2].InvestmentAutomatedTransactionMethod = InvestmentAutomatedTransactionMethod.Withdrawal;
            at[2].NextTransactionDate = new DateTime(2017, 08, 15);
            at[2].RecurringType = Shared.Enums.Core.RecurringType.SemiAnnually;
            at[2].Amount = new decimal(15.50);
            at[2].IsActive = true;
            at[2].PaymentMethod = new PaymentMethod();
            at[2].PaymentMethodId = at[2].PaymentMethod.Id;
            at[2].PaymentMethod.Description = "Checking";


            List<InvestmentAutomatedTransaction> automatedTransactions = new List<InvestmentAutomatedTransaction>();

            foreach (InvestmentAutomatedTransaction row in at)
            {
                automatedTransactions.Add(row);
            }


            var response = new DataResponse<List<InvestmentAutomatedTransaction>>()
            {
                Data = automatedTransactions,
                IsSuccessful = true
            };

            return response;
        }

       
        
        #endregion

        #region Private Methods

      

        #endregion
    }
}
