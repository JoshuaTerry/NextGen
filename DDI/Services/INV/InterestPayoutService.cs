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
    public class InterestPayoutService : ServiceBase<InterestPayout>
    {
        #region Private Fields

        private readonly IRepository<InterestPayout> _repository;
        // private readonly InvestmentRelationshipLogic _investmentrelationshiplogic; 

        #endregion

        #region Constructors

        public InterestPayoutService()
            : this(new UnitOfWorkEF())
        {
        }

        public InterestPayoutService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<InterestPayout>())
        {
        }

        private InterestPayoutService(IUnitOfWork uow, IRepository<InterestPayout> repository)
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

        public IDataResponse<List<InterestPayout>> GetInterestPayoutByInvestmentId(Guid Id)
        {


            InterestPayout[] intPayout;
            intPayout = new InterestPayout[3];

            intPayout[0] = new InterestPayout();
            intPayout[0].Constituent = new Constituent();
            intPayout[0].Constituent.Name = "Joe Smith";
            intPayout[0].Priority = 1;
            intPayout[0].InterestPaymentMethod = InterestPaymentMethod.ACH;
            intPayout[0].Percent = new decimal(50);

            intPayout[1] = new InterestPayout();
            intPayout[1].Constituent = new Constituent();
            intPayout[1].Constituent.Name = "Jane Smith";
            intPayout[1].Priority = 2;
            intPayout[1].InterestPaymentMethod = InterestPaymentMethod.ACH;
            intPayout[1].Percent = new decimal(30);

            intPayout[2] = new InterestPayout();
            intPayout[2].Constituent = new Constituent();
            intPayout[2].Constituent.Name = "Jack Smith";
            intPayout[2].Priority = 3;
            intPayout[2].InterestPaymentMethod = InterestPaymentMethod.ACH;
            intPayout[2].Percent = new decimal(20);

            List<InterestPayout> interestPayouts = new List<InterestPayout>();

            foreach (InterestPayout row in intPayout)
            {
                interestPayouts.Add(row);
            }


            var response = new DataResponse<List<InterestPayout>>()
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
