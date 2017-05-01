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
    public class InvestmentIRSInformationService : ServiceBase<InvestmentIRSInformation>
    {
        #region Private Fields

        private readonly IRepository<InvestmentIRSInformation> _repository;
        
        #endregion

        #region Constructors

        public InvestmentIRSInformationService()
            : this(new UnitOfWorkEF())
        {
        }

        public InvestmentIRSInformationService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<InvestmentIRSInformation>())
        {
        }

        private InvestmentIRSInformationService(IUnitOfWork uow, IRepository<InvestmentIRSInformation> repository)
            : base(uow)
        {
            _repository = repository;
        }

        #endregion

        #region Public Methods

        
        public IDataResponse<List<InvestmentIRSInformation>> GetIRSInformationByInvestmentId(Guid Id)
        {


            InvestmentIRSInformation[] irsInfo;
            irsInfo = new InvestmentIRSInformation[3];

            irsInfo[0] = new InvestmentIRSInformation();
            irsInfo[0].Year = 2014;
            irsInfo[0].InterestPaid = new decimal(34.54);
            irsInfo[0].InterestWithheld = new decimal(432.2);
            irsInfo[0].PenaltyCharged = new decimal(.32);

            irsInfo[1] = new InvestmentIRSInformation();
            irsInfo[1].Year = 2015;
            irsInfo[1].InterestPaid = new decimal(34.76);
            irsInfo[1].InterestWithheld = new decimal(432.76);
            irsInfo[1].PenaltyCharged = new decimal(.76);

            irsInfo[2] = new InvestmentIRSInformation();
            irsInfo[2].Year = 2016;
            irsInfo[2].InterestPaid = new decimal(34.23);
            irsInfo[2].InterestWithheld = new decimal(432.23);
            irsInfo[2].PenaltyCharged = new decimal(.23);


            List<InvestmentIRSInformation> InvestmentIRSInformations = new List<InvestmentIRSInformation>();

            foreach (InvestmentIRSInformation row in irsInfo)
            {
                InvestmentIRSInformations.Add(row);
            }


            var response = new DataResponse<List<InvestmentIRSInformation>>()
            {
                Data = InvestmentIRSInformations,
                IsSuccessful = true
            };

            return response;
        }

       
        
        #endregion

        #region Private Methods

      

        #endregion
    }
}
