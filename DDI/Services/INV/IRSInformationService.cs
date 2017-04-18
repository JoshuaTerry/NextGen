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
    public class IRSInformationService : ServiceBase<IRSInformation>
    {
        #region Private Fields

        private readonly IRepository<IRSInformation> _repository;
        // private readonly InvestmentRelationshipLogic _investmentrelationshiplogic; 

        #endregion

        #region Constructors

        public IRSInformationService()
            : this(new UnitOfWorkEF())
        {
        }

        public IRSInformationService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<IRSInformation>())
        {
        }

        private IRSInformationService(IUnitOfWork uow, IRepository<IRSInformation> repository)
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

        public IDataResponse<List<IRSInformation>> GetIRSInformationByInvestmentId(Guid Id)
        {


            IRSInformation[] irsInfo;
            irsInfo = new IRSInformation[3];

            irsInfo[0] = new IRSInformation();
            irsInfo[0].Year = 2014;
            irsInfo[0].InterestPaid = new decimal(34.54);
            irsInfo[0].InterestWithheld = new decimal(432.2);
            irsInfo[0].PenaltyCharged = new decimal(.32);

            irsInfo[1] = new IRSInformation();
            irsInfo[1].Year = 2015;
            irsInfo[1].InterestPaid = new decimal(34.76);
            irsInfo[1].InterestWithheld = new decimal(432.76);
            irsInfo[1].PenaltyCharged = new decimal(.76);

            irsInfo[2] = new IRSInformation();
            irsInfo[2].Year = 2016;
            irsInfo[2].InterestPaid = new decimal(34.23);
            irsInfo[2].InterestWithheld = new decimal(432.23);
            irsInfo[2].PenaltyCharged = new decimal(.23);


            List<IRSInformation> IRSInformations = new List<IRSInformation>();

            foreach (IRSInformation row in irsInfo)
            {
                IRSInformations.Add(row);
            }


            var response = new DataResponse<List<IRSInformation>>()
            {
                Data = IRSInformations,
                IsSuccessful = true
            };

            return response;
        }

       
        
        #endregion

        #region Private Methods

      

        #endregion
    }
}
