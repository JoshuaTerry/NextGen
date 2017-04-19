using DDI.Business.CRM;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Search;
using DDI.Search.Models;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Enums.INV;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.INV;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace DDI.Services
{
    public class AutomatedTransactionService : ServiceBase<AutomatedTransaction>
    {
        #region Private Fields

        private readonly IRepository<AutomatedTransaction> _repository;
        // private readonly InvestmentRelationshipLogic _investmentrelationshiplogic; 

        #endregion

        #region Constructors

        public AutomatedTransactionService()
            : this(new UnitOfWorkEF())
        {
        }

        public AutomatedTransactionService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<AutomatedTransaction>())
        {
        }

        private AutomatedTransactionService(IUnitOfWork uow, IRepository<AutomatedTransaction> repository)
            : base(uow)
        {
           // _investmentrelationshiplogic = investmentRelationshipLogic;
            _repository = repository;
        }

        #endregion

        #region Public Methods

        //protected override Action<Constituent> FormatEntityForGet => p => SetDateTimeKind(p, q => q.ConstituentStatusDate);


        //public new IDataResponse<List<Investment>> GetAll(string fields, IPageable search)
        //{
        //    // new only for test

        //    List<Investment> invest = MockData();

        //    var response = new DataResponse<List<Investment>>()
        //    {
        //        Data = invest,
        //        IsSuccessful = true
        //    };

        //    return response;
        //}

        //public IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum)
        //{
        //    var constituent = _repository.Entities.FirstOrDefault(c => c.ConstituentNumber == constituentNum);
        //    constituent = _constituentlogic.ConvertAgeRange(constituent);
        //    return GetById(constituent?.Id ?? Guid.Empty);
        //}

        public IDataResponse<AutomatedTransaction> GetAutomatedTransactionById(Guid Id)
        {
            AutomatedTransaction at = new AutomatedTransaction();
            at.Id = new Guid();
            at.AutomatedTransactionMethod = AutomatedTransactionMethod.Deposit;
            at.NextTransactionDate = new DateTime(2017, 08, 15);
            at.RecurringType = Shared.Enums.Core.RecurringType.Monthly;
            at.Amount = new decimal(550);
            at.IsActive = true;
            at.PaymentMethod = "Deposit from Tom Ambler";

            
            var response = new DataResponse<AutomatedTransaction>()
            {
                Data = at,
                IsSuccessful = true
            };

            return response;
        }




        public IDataResponse<List<AutomatedTransaction>> GetAutomatedTransactionByInvestmentId(Guid Id)
        {


            AutomatedTransaction[] at;
            at = new AutomatedTransaction[3];

            at[0] = new AutomatedTransaction();
            at[0].Id = new Guid();
            at[0].AutomatedTransactionMethod = AutomatedTransactionMethod.Deposit;
            at[0].NextTransactionDate = new DateTime(2017, 08, 15);
            at[0].RecurringType = Shared.Enums.Core.RecurringType.Monthly;
            at[0].Amount = new decimal(550);
            at[0].IsActive = true;
            at[0].PaymentMethod = "Deposit from Tom Ambler";

            at[1] = new AutomatedTransaction();
            at[1].Id = new Guid();
            at[1].AutomatedTransactionMethod = AutomatedTransactionMethod.Transfer;
            at[1].NextTransactionDate = new DateTime(2017, 08, 15);
            at[1].RecurringType = Shared.Enums.Core.RecurringType.Weekly;
            at[1].Amount = new decimal(550);
            at[1].IsActive = true;
            at[1].PaymentMethod = "Transfer to Loan 123";

            at[2] = new AutomatedTransaction();
            at[2].Id = new Guid();
            at[2].AutomatedTransactionMethod = AutomatedTransactionMethod.Withdrawal;
            at[2].NextTransactionDate = new DateTime(2017, 08, 15);
            at[2].RecurringType = Shared.Enums.Core.RecurringType.SemiAnnually;
            at[2].Amount = new decimal(15.50);
            at[2].IsActive = true;
            at[2].PaymentMethod = "Check to Jane Doe";
        
            List<AutomatedTransaction> automatedTransactions = new List<AutomatedTransaction>();

            foreach (AutomatedTransaction row in at)
            {
                automatedTransactions.Add(row);
            }


            var response = new DataResponse<List<AutomatedTransaction>>()
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
