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
    public class LinkedAccountService : ServiceBase<LinkedAccount>
    {
        #region Private Fields

        private readonly IRepository<LinkedAccount> _repository;
        // private readonly InvestmentRelationshipLogic _investmentrelationshiplogic; 

        #endregion

        #region Constructors

        public LinkedAccountService()
            : this(new UnitOfWorkEF())
        {
        }

        public LinkedAccountService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<LinkedAccount>())
        {
        }

        private LinkedAccountService(IUnitOfWork uow, IRepository<LinkedAccount> repository)
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

        public IDataResponse<LinkedAccount> GetLinkedAccountById(Guid Id)
        {

            LinkedAccount la = new LinkedAccount();
            la.LinkedAccountType = LinkedAccountType.DownPayment;
            la.LinkedAccountNumber = 123465;

            
            var response = new DataResponse<LinkedAccount>()
            {
                Data = la,
                IsSuccessful = true
            };

            return response;
        }


        public IDataResponse<List<LinkedAccount>> GetLinkedAccountByInvestmentId(Guid Id)
        {


            LinkedAccount[] la;
            la = new LinkedAccount[2];

            la[0] = new LinkedAccount();
            la[0].LinkedAccountType = LinkedAccountType.DownPayment;
            la[0].LinkedAccountNumber = 123465;

            la[1] = new LinkedAccount();
            la[1].LinkedAccountType = LinkedAccountType.Pool;
            la[1].LinkedAccountNumber = 4007;

            List<LinkedAccount> linkedAccounts = new List<LinkedAccount>();

            foreach (LinkedAccount row in la)
            {
                linkedAccounts.Add(row);
            }


            var response = new DataResponse<List<LinkedAccount>>()
            {
                Data = linkedAccounts,
                IsSuccessful = true
            };

            return response;
        }

       
        
        #endregion

        #region Private Methods

      

        #endregion
    }
}
