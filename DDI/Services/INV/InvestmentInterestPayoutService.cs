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
    public class InvestmentInterestPayoutService : ServiceBase<InvestmentInterestPayout>
    {
        #region Private Fields

        private readonly IRepository<InvestmentInterestPayout> _repository;
        
        #endregion

        #region Constructors

        public InvestmentInterestPayoutService()
            : this(new UnitOfWorkEF())
        {
        }

        public InvestmentInterestPayoutService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<InvestmentInterestPayout>())
        {
        }

        private InvestmentInterestPayoutService(IUnitOfWork uow, IRepository<InvestmentInterestPayout> repository)
            : base(uow)
        {
           // _investmentrelationshiplogic = investmentRelationshipLogic;
            _repository = repository;
        }

        #endregion

        #region Public Methods

        
        public IDataResponse<InvestmentInterestPayout> GetInterestPayoutById(Guid Id)
        {


            InvestmentInterestPayout intPayout = new InvestmentInterestPayout();
            intPayout.Constituent = new Constituent();
            intPayout.Constituent.Name = "Joe Smith";
            intPayout.Priority = 1;
            intPayout.InterestPaymentMethod = InterestPaymentMethod.EFT;
            intPayout.Percent = new decimal(50);

            
            var response = new DataResponse<InvestmentInterestPayout>()
            {
                Data = intPayout,
                IsSuccessful = true
            };

            return response;
        }


        public IDataResponse<List<InvestmentInterestPayout>> GetInterestPayoutByInvestmentId(Guid Id)
        {


            InvestmentInterestPayout[] intPayout;
            intPayout = new InvestmentInterestPayout[3];

            intPayout[0] = new InvestmentInterestPayout();
            intPayout[0].Constituent = new Constituent();
            intPayout[0].Constituent.Name = "Joe Smith";
            intPayout[0].Priority = 1;
            intPayout[0].InterestPaymentMethod = InterestPaymentMethod.EFT;
            intPayout[0].Percent = new decimal(50);

            intPayout[1] = new InvestmentInterestPayout();
            intPayout[1].Constituent = new Constituent();
            intPayout[1].Constituent.Name = "Jane Smith";
            intPayout[1].Priority = 2;
            intPayout[1].InterestPaymentMethod = InterestPaymentMethod.EFT;
            intPayout[1].Percent = new decimal(30);

            intPayout[2] = new InvestmentInterestPayout();
            intPayout[2].Constituent = new Constituent();
            intPayout[2].Constituent.Name = "Jack Smith";
            intPayout[2].Priority = 3;
            intPayout[2].InterestPaymentMethod = InterestPaymentMethod.EFT;
            intPayout[2].Percent = new decimal(20);

            List<InvestmentInterestPayout> interestPayouts = new List<InvestmentInterestPayout>();

            foreach (InvestmentInterestPayout row in intPayout)
            {
                interestPayouts.Add(row);
            }


            var response = new DataResponse<List<InvestmentInterestPayout>>()
            {
                Data = interestPayouts,
                IsSuccessful = true
            };

            return response;
        }

       
        
        #endregion

        #region Private Methods

      

        #endregion
    }
}
