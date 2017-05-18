using System;
using System.Collections.Generic;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.INV;

namespace DDI.Services
{
    public class InvestmentRelationshipService : ServiceBase<InvestmentRelationship>
    {
        #region Private Fields

        private readonly IRepository<InvestmentRelationship> _repository;
        
        #endregion

        #region Constructors

        public InvestmentRelationshipService()
            : this(new UnitOfWorkEF())
        {
        }

        public InvestmentRelationshipService(IUnitOfWork uow)
            : this(uow,  uow.GetRepository<InvestmentRelationship>())
        {
        }

        private InvestmentRelationshipService(IUnitOfWork uow, IRepository<InvestmentRelationship> repository)
            : base(uow)
        {
            _repository = repository;
        }

        #endregion

        #region Public Methods

        
        public new IDataResponse<List<InvestmentRelationship>> GetAll(string fields, IPageable search)
        {
            List<InvestmentRelationship> invrelationship = MockData();

            var response = new DataResponse<List<InvestmentRelationship>>()
            {
                Data = invrelationship,
                IsSuccessful = true
            };

            return response;
        }

        
        public IDataResponse<List<InvestmentRelationship>> GetInvestmentByConstituentId(Guid constituentId)
        {
            List<InvestmentRelationship> mockInvestmentRelationship1 = MockData();

            var response = new DataResponse<List<InvestmentRelationship>>()
            {
                Data = mockInvestmentRelationship1,
                IsSuccessful = true
            };

            return response;
        }

        private List<InvestmentRelationship> MockData()
        {
            InvestmentRelationship[] invrelate;
            invrelate = new InvestmentRelationship[10];

            invrelate[0] = new InvestmentRelationship();
            invrelate[0].Investment = new Investment();
            invrelate[0].Investment.InvestmentNumber = 123456;
            invrelate[0].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[0].Investment.InvestmentOwnershipType.Code = "S";
            invrelate[0].Investment.InvestmentOwnershipType.Name = "Sole Ownership";
            invrelate[0].Investment.CurrentMaturityDate = new DateTime(2000, 05, 04);
            invrelate[0].Investment.Rate = new decimal(4.25);
            invrelate[0].InvestmentId = Guid.NewGuid();
            invrelate[0].InvestmentRelationshipType = InvestmentRelationshipType.Beneficiary;
            //invrelate[0].InvestmentRelationshipTypeDescription = InvestmentRelationshipType.Beneficiary;

            invrelate[1] = new InvestmentRelationship();
            invrelate[1].Investment = new Investment();
            invrelate[1].Investment.InvestmentNumber = 234567;
            invrelate[1].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[1].Investment.InvestmentOwnershipType.Code = "J";
            invrelate[1].Investment.InvestmentOwnershipType.Name = "Joint Ownership";
            invrelate[1].Investment.CurrentMaturityDate = new DateTime(2001, 05, 04);
            invrelate[1].Investment.Rate = new decimal(4.50);
            invrelate[1].InvestmentId = Guid.NewGuid();
            invrelate[1].InvestmentRelationshipType = InvestmentRelationshipType.Beneficiary;

            invrelate[2] = new InvestmentRelationship();
            invrelate[2].Investment = new Investment();
            invrelate[2].Investment.InvestmentNumber = 345678;
            invrelate[2].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[2].Investment.InvestmentOwnershipType.Code = "S";
            invrelate[2].Investment.InvestmentOwnershipType.Name = "Sole Ownership";
            invrelate[2].Investment.CurrentMaturityDate = new DateTime(2002, 05, 04);
            invrelate[2].Investment.Rate = new decimal(4.75);
            invrelate[2].InvestmentId = Guid.NewGuid();
            invrelate[2].InvestmentRelationshipType = InvestmentRelationshipType.Beneficiary;

            invrelate[3] = new InvestmentRelationship();
            invrelate[3].Investment = new Investment();
            invrelate[3].Investment.InvestmentNumber = 456789;
            invrelate[3].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[3].Investment.InvestmentOwnershipType.Code = "J";
            invrelate[3].Investment.InvestmentOwnershipType.Name = "Joint Ownership";
            invrelate[3].Investment.CurrentMaturityDate = new DateTime(2003, 05, 04);
            invrelate[3].Investment.Rate = new decimal(5);
            invrelate[3].InvestmentId = Guid.NewGuid();
            invrelate[3].InvestmentRelationshipType = InvestmentRelationshipType.Joint;

            invrelate[4] = new InvestmentRelationship();
            invrelate[4].Investment = new Investment();
            invrelate[4].Investment.InvestmentNumber = 567890;
            invrelate[4].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[4].Investment.InvestmentOwnershipType.Code = "S";
            invrelate[4].Investment.InvestmentOwnershipType.Name = "Sole Ownership";
            invrelate[4].Investment.CurrentMaturityDate = new DateTime(2004, 05, 04);
            invrelate[4].Investment.Rate = new decimal(5.25);
            invrelate[4].InvestmentId = Guid.NewGuid();
            invrelate[4].InvestmentRelationshipType = InvestmentRelationshipType.Joint;

            invrelate[5] = new InvestmentRelationship();
            invrelate[5].Investment = new Investment();
            invrelate[5].Investment.InvestmentNumber = 678901;
            invrelate[5].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[5].Investment.InvestmentOwnershipType.Code = "J";
            invrelate[5].Investment.InvestmentOwnershipType.Name = "Joint Ownership";
            invrelate[5].Investment.CurrentMaturityDate = new DateTime(2005, 05, 04);
            invrelate[5].Investment.Rate = new decimal(5.5);
            invrelate[5].InvestmentId = Guid.NewGuid();
            invrelate[5].InvestmentRelationshipType = InvestmentRelationshipType.Joint;

            invrelate[6] = new InvestmentRelationship();
            invrelate[6].Investment = new Investment();
            invrelate[6].Investment.InvestmentNumber = 789012;
            invrelate[6].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[6].Investment.InvestmentOwnershipType.Code = "S";
            invrelate[6].Investment.InvestmentOwnershipType.Name = "Sole Ownership";
            invrelate[6].Investment.CurrentMaturityDate = new DateTime(2006, 05, 04);
            invrelate[6].Investment.Rate = new decimal(5.75);
            invrelate[6].InvestmentId = Guid.NewGuid();
            invrelate[6].InvestmentRelationshipType = InvestmentRelationshipType.Primary;

            invrelate[7] = new InvestmentRelationship();
            invrelate[7].Investment = new Investment();
            invrelate[7].Investment.InvestmentNumber = 890123;
            invrelate[7].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[7].Investment.InvestmentOwnershipType.Code = "J";
            invrelate[7].Investment.InvestmentOwnershipType.Name = "Joint Ownership";
            invrelate[7].Investment.CurrentMaturityDate = new DateTime(2007, 05, 04);
            invrelate[7].Investment.Rate = new decimal(6);
            invrelate[7].InvestmentId = Guid.NewGuid();
            invrelate[7].InvestmentRelationshipType = InvestmentRelationshipType.Primary;
        
            invrelate[8] = new InvestmentRelationship();
            invrelate[8].Investment = new Investment();
            invrelate[8].Investment.InvestmentNumber = 901234;
            invrelate[8].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[8].Investment.InvestmentOwnershipType.Code = "S";
            invrelate[8].Investment.InvestmentOwnershipType.Name = "Sole Ownership";
            invrelate[8].Investment.CurrentMaturityDate = new DateTime(2008, 05, 04);
            invrelate[8].Investment.Rate = new decimal(6.25);
            invrelate[8].InvestmentId = Guid.NewGuid();
            invrelate[8].InvestmentRelationshipType = InvestmentRelationshipType.Primary;

            invrelate[9] = new InvestmentRelationship();
            invrelate[9].Investment = new Investment();
            invrelate[9].Investment.InvestmentNumber = 012345;
            invrelate[9].Investment.InvestmentOwnershipType = new InvestmentOwnershipType();
            invrelate[9].Investment.InvestmentOwnershipType.Code = "J";
            invrelate[9].Investment.InvestmentOwnershipType.Name = "Joint Ownership";
            invrelate[9].Investment.CurrentMaturityDate = new DateTime(2009, 05, 04);
            invrelate[9].Investment.Rate = new decimal(6.5);
            invrelate[9].InvestmentId = Guid.NewGuid();
            invrelate[9].InvestmentRelationshipType = InvestmentRelationshipType.Primary;

            List<InvestmentRelationship> investmentRelationshipList = new List<InvestmentRelationship>();

            foreach (InvestmentRelationship row in invrelate)
            {
                investmentRelationshipList.Add(row);
            }

            return investmentRelationshipList;
        }
       
        
        #endregion

        #region Private Methods

      

        #endregion
    }
}
