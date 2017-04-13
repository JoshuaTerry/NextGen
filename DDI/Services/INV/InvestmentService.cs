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
    public class InvestmentService : ServiceBase<Investment>
    {
        #region Private Fields

        private readonly IRepository<Investment> _repository;
        // private readonly InvestmentRelationshipLogic _investmentrelationshiplogic; 

        #endregion

        #region Constructors

        public InvestmentService()
            : this(new UnitOfWorkEF())
        {
        }

        public InvestmentService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<Investment>())
        {
        }

        private InvestmentService(IUnitOfWork uow, IRepository<Investment> repository)
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

        public IDataResponse<Investment> GetInvestmentById(Guid Id)
        {
            Investment invest = new Investment();
            invest.Id = new Guid();

            //main investment information
            invest.InvestmentNumber = 123456;
            invest.InvestmentType = new InvestmentType();
            invest.InvestmentType.Type = 403;
            invest.InvestmentType.Description = "Fund";
            invest.InvestmentOwnershipType = new InvestmentOwnershipType();
            invest.InvestmentOwnershipType.Code = "JT";
            invest.InvestmentOwnershipType.Name = "Joint Tenancy";
            invest.InvestmentStatus = InvestmentStatus.Current;
            invest.CUSIP = "A4E8G4";

            //attributes section
            invest.PurchaseDate = new DateTime(2005, 12, 13);
            invest.IssuanceMethod = IssuanceMethod.BookEntry;
            invest.OriginalPurchaseAmount = new decimal (2345.43);
            invest.StepUpEligible = true;
            invest.StepUpDate = new DateTime (2017, 07,12);
                
            //interest section
            



            var response = new DataResponse<Investment>()
            {
                Data = invest,
                IsSuccessful = true
            };

            return response;
        }

       
        
        #endregion

        #region Private Methods

      

        #endregion
    }
}
