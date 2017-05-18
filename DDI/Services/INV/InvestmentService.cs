using System;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.INV;

namespace DDI.Services
{
    public class InvestmentService : ServiceBase<Investment>
    {
        #region Private Fields

        private readonly IRepository<Investment> _repository;
        
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
            _repository = repository;
        }

        #endregion

        #region Public Methods

        
        public IDataResponse<Investment> GetInvestmentById(Guid Id)
        {
            Investment invest = new Investment();
            invest.Id = new Guid();

            //main investment information
            invest.InvestmentNumber = 123456;
            invest.InvestmentType = new InvestmentType();
            invest.InvestmentType.Code = "403";
            invest.InvestmentType.Name = "Fund";
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
            invest.InterestFrequency = InterestFrequency.Monthly;
            invest.Rate = new decimal(4.5);
            




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
