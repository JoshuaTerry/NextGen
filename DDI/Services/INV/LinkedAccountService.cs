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
            _repository = repository;
        }

        #endregion

        #region Public Methods

        
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
